using System.Collections.Generic;
using System.IO;

namespace CRAT.Infrastructure.File
{
    public static class FileReader
    {
        public static List<string> LoadFile(string path)
        {
            List<string> data = new List<string>();

            //  File doesn't exist
            if (!System.IO.File.Exists(path))
                return data;

            using StreamReader reader = new StreamReader(path);

            //  File is empty
            if (reader.EndOfStream)
                return data;

            //  Load into data
            for (int i = 0; !reader.EndOfStream; i++)
                data.Add(reader.ReadLine());

            return data;
        }
    }
}
