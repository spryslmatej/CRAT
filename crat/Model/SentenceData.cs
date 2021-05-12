using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CRAT.Model
{
	/// <summary>
	/// Class for containing, preparing, and working with Tokens, Annotations and Relations
	/// </summary>
	public class SentenceData
	{
		public ObservableCollection<Token> Tokens { get; private set; }
		public ObservableCollection<Annotation> Annotations { get; private set; }
		public ObservableCollection<Relation> Relations { get; private set; }

		public SentenceData(
			List<Token> t,
			List<Annotation> a,
			List<Relation> r)
		{
			if (t is null || a is null || r is null)
				throw new ArgumentException("Lists cannot be null.");

			Tokens = new ObservableCollection<Token>(t);
			Annotations = new ObservableCollection<Annotation>(a);
			Relations = new ObservableCollection<Relation>(r);
		}

		public SentenceData(string s = "")
		{
			if (s is null)
				throw new ArgumentException("Sentence cannot be empty.");

			Tokens = new ObservableCollection<Token>(GetTokens(s));
			Annotations = new ObservableCollection<Annotation>(GetAnnotations());
			Relations = new ObservableCollection<Relation>(GetRelations());
		}

		#region Public Methods -- Collection Altering

		public void AddAnnotation(AnnotationTemplate template, int index)
		{
			Annotations[index] = new Annotation(index, template.Text);
		}
		public void RemoveAnnotation(int index)
		{
			//  Remove relations connected to removed annotation
			for (int i = 0; i < Relations.Count; i++)
			{
				var item = Relations[i];
				if (item.LeftIndex == index || item.RightIndex == index)
				{
					RemoveRelation(i);
					i--;
				}
			}

			//  Remove annotation
			Annotations[index] = null;
		}
		public bool AddRelation(RelationTemplate template, int srcIndex, int destIndex)
		{
			if (
				(template.SourceAnnotations.Count != 0 && !template.SourceAnnotations.Contains(Annotations[srcIndex].Text)) ||
				(template.DestinationAnnotations.Count != 0 && !template.DestinationAnnotations.Contains(Annotations[destIndex].Text))
				)
				return false;

			Relations.Add(new Relation(template, srcIndex, destIndex));
			return true;
		}
		public void RemoveRelation(int index)
		{
			Relations.RemoveAt(index);
		}

		#endregion

		#region Private Methods -- Field Generators

		private List<string> SplitText(string s)
		{
			// https://stackoverflow.com/questions/4680128/split-a-string-with-delimiters-but-keep-the-delimiters-in-the-result-in-c-sharp

			string[] substrings = Regex.Split(s, AppConfig.Config.SplitRegexPattern);

			return new List<string>(substrings);
		}

		private List<Token> GetTokens(string s)
		{
			List<string> stringTokens = SplitText(s);

			stringTokens.RemoveAll(item => Regex.Replace(item, @"\s+", "") == "");

			List<Token> output = new List<Token>();

			foreach (string item in stringTokens)
			{
				output.Add(new Token(item));
			}

			return output;
		}
		private List<Annotation> GetAnnotations()
		{
			var output = new List<Annotation>();
			for (int i = 0; i < Tokens.Count; i++)
				output.Add(null);
			return output;
		}
		private List<Relation> GetRelations()
		{
			return new List<Relation>();
		}
		#endregion

	}
}
