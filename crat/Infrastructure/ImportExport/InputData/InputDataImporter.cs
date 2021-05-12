using CRAT.Infrastructure.File;
using System;

namespace CRAT.Infrastructure.ImportExport
{
	public static class InputDataImporter
	{
		public static string ImportInputData(string path)
		{
            var data = FileReader.LoadFile(path);
            if (data.Count == 0)
                throw new ArgumentException("File is either empty or does not exist.");

			return data[0];
        }
	}
}
