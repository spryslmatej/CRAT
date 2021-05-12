using CRAT.Infrastructure.File;
using CRAT.Model;
using System.Text;
using System.Text.Json;

namespace CRAT.Infrastructure.ImportExport
{
	public static class ConfigImporter
	{
		public static bool LoadConfig()
		{
			var file = FileReader.LoadFile(AppConfig.Config.ConfigPath);

			if (file.Count == 0)
				return false;

			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				AllowTrailingCommas = true,
				IgnoreNullValues = true
			};

			var data = new StringBuilder();

			file.ForEach(item => data.Append(item));

			try
			{
				AppConfig.Config = JsonSerializer.Deserialize<AppConfig>(data.ToString(), options);
				return true;
			}
			catch { return false; }
		}
	}
}
