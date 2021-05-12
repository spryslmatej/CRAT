using CRAT.Model;
using System.Collections.Generic;
using System.Text;

namespace CRAT.Infrastructure.ImportExport
{
	public static class SentenceExporter
    {
        public static List<string> ExportSentence(SentenceData s)
        {
            List<string> data = new List<string>();

            var delimiter = AppConfig.Config.SentenceFileDelimiter;

            /*
             * Use of StringBuilder: https://rules.sonarsource.com/csharp/RSPEC-1643
             */

            //  Tokens
            StringBuilder tokens = new StringBuilder();
            foreach (Token t in s.Tokens)
            {
                tokens.Append(t.ToString() + delimiter);
            }
            data.Add(tokens.ToString());

            //  Annotations
            StringBuilder annotations = new StringBuilder();
            foreach (Annotation a in s.Annotations)
            {
                if (a is null)
                    continue;
                annotations.Append(a.ToString() + delimiter);
            }
            data.Add(annotations.ToString());

            //  Relations

            StringBuilder relations = new StringBuilder();
            foreach (Relation r in s.Relations)
            {
                relations.Append(r.ToString() + delimiter);
            }
            data.Add(relations.ToString());

            return data;
        }
    }
}
