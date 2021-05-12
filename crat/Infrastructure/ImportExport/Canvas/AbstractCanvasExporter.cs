using CRAT.Control;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Shapes;

namespace CRAT.Infrastructure.ImportExport
{
	public abstract class AbstractCanvasExporter
	{
		public readonly NumberFormatInfo nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };
		public List<string> Export(
			IEnumerable<SelectableTextBlock> tokens,
			IEnumerable<SelectableTextBlock> annotations,
			IEnumerable<List<UIElement>> relations,
			IEnumerable<Path> brackets,
			Size size
			)
		{
			Size svgSize = new Size((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));

			List<string> output = new List<string>
			{
				@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
                    <!DOCTYPE svg PUBLIC "" -//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd "">",
				$@"<svg xmlns = ""http://www.w3.org/2000/svg"" xmlns:xlink= ""http://www.w3.org/1999/xlink"" style=""width: {svgSize.Width}px; height: {svgSize.Height}px "">
                    <defs>
                        <marker id=""arrow_triangle_5_black"" refX=""5"" refY=""2.5"" markerWidth=""5"" markerHeight=""5"" orient=""auto-start-reverse"" markerUnits=""strokeWidth"" fill=""black"">
                            <polyline points=""0,0 5,2.5 0,5 0.4166666666666667, 2.5""/>
                        </marker>
                    </defs>"
			};

			//  Tokens
			List<string> tokSVG = GetTokensInSVG(tokens);
			foreach (string item in tokSVG)
				output.Add(item);

			//  Annotations
			List<string> annSVG = GetAnnotationsInSVG(annotations);
			foreach (string item in annSVG)
				output.Add(item);

			//  Relations
			List<string> relSVG = GetRelationsInSVG(relations);
			foreach (string item in relSVG)
				output.Add(item);

			//  Brackets
			foreach (var item in brackets)
				if (!(item is null))
					output.Add(PathToSVG(item, false));

			output.Add("</svg>");

			return output;
		}


		#region Public Methods -- UIElements Conversion
		public abstract string SelectableTextBlockToSVG(SelectableTextBlock item);
		public abstract string PathToSVG(Path item, bool drawMarker = true);
		public List<string> GetTokensInSVG(IEnumerable<SelectableTextBlock> tokens)
		{
			List<string> output = new List<string> { "<g>" };

			foreach (SelectableTextBlock item in tokens)
			{
				string s = SelectableTextBlockToSVG(item);
				output.Add(s);
			}
			output.Add("</g>");
			return output;
		}
		public List<string> GetAnnotationsInSVG(IEnumerable<SelectableTextBlock> annotations)
		{
			List<string> output = new List<string> { "<g>" };

			foreach (SelectableTextBlock item in annotations)
			{
				if (item is null)
					continue;

				string s = "";
				s += "<g>";

				//  Text
				s += SelectableTextBlockToSVG(item);

				s += "</g>";
				output.Add(s);
			}
			output.Add("</g>");
			return output;
		}
		public List<string> GetRelationsInSVG(IEnumerable<List<UIElement>> relations)
		{
			List<string> output = new List<string> { "<g>" };

			foreach (var relation in relations)
			{
				foreach (var element in relation)
				{
					//  Lines
					if (element is Path path)
					{
						output.Add(PathToSVG(path));
					}
					//  Text
					else if (relation.Find(item => item is SelectableTextBlock) is SelectableTextBlock text)
					{
						output.Add(SelectableTextBlockToSVG(text));
					}
				}
			}
			output.Add("</g>");
			return output;
		}
		#endregion
	}
}
