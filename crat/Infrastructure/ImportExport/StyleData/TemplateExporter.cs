using CRAT.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CRAT.Infrastructure.ImportExport
{
	public static class TemplateExporter
    {
        public static List<string> ExportAnnotations(ObservableCollection<AnnotationTemplate> annotations)
        {
            //  Prepare data
            string[] lines = new string[annotations.Count];
            for (int i = 0; i < annotations.Count; i++)
                lines[i] = annotations[i].ToString();

            return new List<string>(lines);
        }

        public static List<string> ExportRelations(ObservableCollection<RelationTemplate> relations)
        {
            //  Prepare data
            string[] lines = new string[relations.Count];
            for (int i = 0; i < relations.Count; i++)
                lines[i] = relations[i].ToString();

            return new List<string>(lines);
        }
    }
}
