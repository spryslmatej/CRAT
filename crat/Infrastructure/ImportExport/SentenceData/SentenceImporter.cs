using CRAT.Infrastructure.File;
using CRAT.Model;
using System;
using System.Collections.Generic;

namespace CRAT.Infrastructure.ImportExport
{
	public static class SentenceImporter
	{
		public static SentenceData ImportSentence(string path)
		{
			var data = FileReader.LoadFile(path);
			if (data.Count == 0)
				throw new ArgumentException("File is either empty or does not exist.");

			var delimiter = AppConfig.Config.SentenceFileDelimiter;

			//  Tokens
			List<Token> tokens = new List<Token>();
			string[] tokensString = data[0].Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < tokensString.Length; i++)
			{
				tokens.Add(new Token(tokensString[i]));
			}

			//  Annotations
			List<Annotation> annotations = new List<Annotation>();
			for (int i = 0; i < tokens.Count; i++)
				annotations.Add(null);
			string[] annotationsString = data[1].Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < annotationsString.Length; i++)
			{
				var curAnno = Annotation.FromString(annotationsString[i]);
				if (!(curAnno is null))
					annotations[curAnno.TokenIndex] = curAnno;
			}

			//  Relations
			List<Relation> relations = new List<Relation>();
			string[] relationsString = data[2].Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < relationsString.Length; i++)
			{
				var curRel = Relation.FromString(relationsString[i]);
				if (!(curRel is null))
					relations.Add(curRel);
			}

			return new SentenceData(tokens, annotations, relations);
		}
	}
}
