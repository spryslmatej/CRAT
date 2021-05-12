using CRAT.Infrastructure.File;
using CRAT.Model;
using System.Collections.Generic;

namespace CRAT.Infrastructure.ImportExport
{
	public static class TemplateImporter
    {
        public static List<AnnotationTemplate> ImportAnnotations(string path)
        {
            List<string> data = FileReader.LoadFile(path);

            List<AnnotationTemplate> output = new List<AnnotationTemplate>();

            //  Empty or non existent file
            if (data.Count == 0)
                return output;

            //  Try to parse
            try
            {
                foreach (string item in data)
                    output.Add(AnnotationTemplate.FromString(item));
            }
            catch
            {
                //  Incorrect file
                return new List<AnnotationTemplate>();
            }

            //  Correct file
            return output;
        }

        public static List<RelationTemplate> ImportRelations(string path)
        {
            List<string> data = FileReader.LoadFile(path);

            List<RelationTemplate> output = new List<RelationTemplate>();

            //  Empty or non existent file
            if (data == null)
                return output;

            //  Try to parse
            try
            {
                foreach (string item in data)
                    output.Add(RelationTemplate.FromString(item));
            }
            catch
            {
                //  Incorrect file
                return new List<RelationTemplate>();
            }

            //  Correct file
            return output;
        }
    }
}
