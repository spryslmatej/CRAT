


namespace CRAT.Model
{
	public class AppConfig
	{
		//	Although this is a singleton, it is not Reflection safe
		//	https://courses.fit.cvut.cz/BI-OOP/resources/lectures/bi-oop-b201-lecture-10-design-patterns.pdf

		private AppConfig() { }

		/// <summary>
		/// Current instance of AppConfig pseudosingleton.
		/// Do not assign this to a variable!
		/// Always access fields through this instance.
		/// </summary>
		public static AppConfig Config { get; set; } = new AppConfig();


		public readonly string ConfigPath = "CRAT.config";

		public string SplitRegexPattern { get; set; } = "([ ]|[,]|[.]|[(]|[)]|[;])";
		public char SentenceFileDelimiter { get; set; } = ';';

		public string AnnotationTemplateDefaultFilePath { get; set; } = "./annotations.dat";
		public string RelationTemplateDefaultFilePath { get; set; } = "./relations.dat";

		#region Drawing
		public double MinimumBoxSize { get; set; } = 10d;
		public double RelationEndPointMove { get; set; } = 3d;
		public double CanvasMargin { get; set; } = 5d;
		public double TokensDefaultGap { get; set; } = 5d;
		public double RelationsLevelGap { get; set; } = 12d;
		public double FixedRelationOffset { get; set; } = 8d;

		public int FontSize_Tokens { get; set; } = 12;
		public int FontSize_Annotations { get; set; } = 12;
		public int FontSize_Relations { get; set; } = 10;

		public double BackgroundOpacity_Tokens { get; set; } = 0.1;
		public double BackgroundOpacity_Annotations { get; set; } = 0.2;

		public double BracketOpacity { get; set; } = 0.6;
		public double BracketThickness { get; set; } = 1.5d;

		public double RelationLineThickness { get; set; } = 0.8d;
		public string RelationLineColor { get; set; } = "#000000";

		public double SelectedTextBlock_Opacity { get; set; } = 0.4d;
		public string SelectedTextBlock_Color { get; set; } = "#00FFFF";

		#endregion

	}
}
