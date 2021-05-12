using CRAT.Control;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CRAT.Infrastructure.ImportExport
{
	public class ColorlessSvgExporter : AbstractCanvasExporter
	{
		public override string SelectableTextBlockToSVG(SelectableTextBlock item)
		{
			var offsetX = item.RenderTransform.Value.OffsetX;
			var offsetY = item.RenderTransform.Value.OffsetY;
			var height = item.DesiredSize.Height;
			var width = item.DesiredSize.Width;

			string s = "";

			//  Background rectangle
			if (!(item.DefaultBackground is null))
				s += $@"<rect x=""{(offsetX).ToString(nfi)
					}"" y=""{(offsetY).ToString(nfi)
					}"" width=""{width.ToString(nfi)
					}"" height=""{height.ToString(nfi)
					}"" fill=""#888888
					"" opacity=""{item.DefaultBackground.Opacity.ToString(nfi)}"" />";

			//  Text
			s += $@"<text font-family=""{item.FontFamily
				}"" font-size=""{item.FontSize
				}"" text-anchor=""middle"" style=""fill:#000000
				"" x=""{(offsetX + width * 0.5).ToString(nfi)
				}"" y=""{(offsetY + height * 0.75).ToString(nfi)
				}"">{item.Text
				}</text>";

			return s;
		}
		public override string PathToSVG(Path item, bool drawMarker = true)
		{
			var figures = (item.Data as PathGeometry).Figures.ToString(nfi);

			string s = $@"<path d=""{figures
				}"" marker-end=""{(drawMarker ? "url(#arrow_triangle_5_black)" : "")
				}"" style=""stroke:#000000; stroke-width: {item.StrokeThickness.ToString(nfi)
				}; fill: none "" />";

			return s;
		}
	}
}
