using System.Collections.Generic;
using System.IO;

namespace CRAT.Infrastructure.File
{
    public static class FileWriter
    {
        public static void WriteFile(string path, string data)
		{
            WriteFile(path, new List<string>() { data });
		}

        public static void WriteFile(string path, List<string> data)
        {
            using StreamWriter writer = new StreamWriter(path, false);
            foreach (var item in data)
                writer.WriteLine(item);
        }
    }
}
