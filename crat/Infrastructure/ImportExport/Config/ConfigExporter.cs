using CRAT.Infrastructure.File;
using CRAT.Model;
using System.Text.Json;

namespace CRAT.Infrastructure.ImportExport
{
	public static class ConfigExporter
	{
		public static bool WriteConfig()
		{
			var options = new JsonSerializerOptions { WriteIndented = true };

			try
			{
				var config = JsonSerializer.Serialize<AppConfig>(AppConfig.Config, options);
				FileWriter.WriteFile(AppConfig.Config.ConfigPath, config);
				return true;
			}
			catch { return false; }
		}
	}
}
