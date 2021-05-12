using CRAT.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CRAT.Control
{
	public static class UIElementsFactory
	{
		private static readonly Size _unlimitedSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

		private static SelectableTextBlock TokenToTextBlock(Token token)
		{
			var textBlock = new SelectableTextBlock()
			{
				Text = token.Text,
				FontSize = AppConfig.Config.FontSize_Tokens,
				MinWidth = AppConfig.Config.MinimumBoxSize,
				TextAlignment = TextAlignment.Center
			};
			textBlock.Measure(_unlimitedSize);

			return textBlock;
		}
		private static SelectableTextBlock AnnotationToTextBlock(Annotation annotation)
		{
			if (annotation is null)
				return null;

			var textBlock = new SelectableTextBlock()
			{
				Text = annotation.Text,
				FontSize = AppConfig.Config.FontSize_Annotations,
				MinWidth = AppConfig.Config.MinimumBoxSize,
				TextAlignment = TextAlignment.Center
			};
			textBlock.Measure(_unlimitedSize);

			return textBlock;
		}

		public static void TokensAndAnnotationsToTextBlock(
			SentenceData sentenceData,
			out List<SelectableTextBlock> tokens,
			out List<SelectableTextBlock> annotations)
		{
			//  Get annotations
			annotations = new List<SelectableTextBlock>();
			foreach (Annotation item in sentenceData.Annotations)
				annotations.Add(AnnotationToTextBlock(item));

			//  Get tokens
			tokens = new List<SelectableTextBlock>();
			foreach (Token item in sentenceData.Tokens)
				tokens.Add(TokenToTextBlock(item));
		}

		/*
         * Namings are added before the rest, because we need it for arranging Tokens and Annotations
         */
		public static void AddNamingsToRelationDictionary(Dictionary<Relation, List<UIElement>> pairs)
		{
			foreach (var item in pairs)
			{
				var textBlock = new SelectableTextBlock(Brushes.White)
				{
					Text = item.Key.Text,
					FontSize = AppConfig.Config.FontSize_Relations,
					MinWidth = AppConfig.Config.MinimumBoxSize,
					TextAlignment = TextAlignment.Center
				};
				textBlock.Measure(_unlimitedSize);

				item.Value.Add(textBlock);
			}
		}

		public static void AddResemblanceToRelationsToUIElements(Dictionary<Relation, List<UIElement>> pairs, Dictionary<Relation, int> relationsLevelsDict, List<SelectableTextBlock> uiAnnotations)
		{
			foreach (var item in pairs)
			{
				var naming = item.Value.Find(item => item is SelectableTextBlock) as SelectableTextBlock;
				var leftAnnotation = uiAnnotations[item.Key.LeftIndex];
				var rightAnnotation = uiAnnotations[item.Key.RightIndex];

				relationsLevelsDict.TryGetValue(item.Key, out int level);

				var resemblance = RelationResemblanceFactory.RelationToUIElements(item.Key, level, naming, leftAnnotation, rightAnnotation);
				item.Value.Clear();
				item.Value.AddRange(resemblance);
			}
		}

		private static string GetBracket(double x1, double y1, double x2, double y2, double w, double q)
		{
			//  Credits: http://bl.ocks.org/alexhornbake/6005176

			//returns path string d for <path d="This string">
			//a curly brace between x1,y1 and x2,y2, w pixels wide 
			//and q factor, .5 is normal, higher q = more expressive bracket 

			//Calculate unit vector
			var dx = x1 - x2;
			var dy = y1 - y2;
			var len = Math.Sqrt(dx * dx + dy * dy);
			dx /= len;
			dy /= len;

			//Calculate Control Points of path,
			var qx1 = x1 + q * w * dy;
			var qy1 = y1 - q * w * dx;
			var qx2 = (x1 - .25 * len * dx) + (1 - q) * w * dy;
			var qy2 = (y1 - .25 * len * dy) - (1 - q) * w * dx;
			var tx1 = (x1 - .5 * len * dx) + w * dy;
			var ty1 = (y1 - .5 * len * dy) - w * dx;
			var qx3 = x2 + q * w * dy;
			var qy3 = y2 - q * w * dx;
			var qx4 = (x1 - .75 * len * dx) + (1 - q) * w * dy;
			var qy4 = (y1 - .75 * len * dy) - (1 - q) * w * dx;

			NumberFormatInfo nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };

			return ("M " + x1.ToString(nfi) + " " + y1.ToString(nfi) +
					 " Q " + qx1.ToString(nfi) + " " + qy1.ToString(nfi) + " " + qx2.ToString(nfi) + " " + qy2.ToString(nfi) +
					  " T " + tx1.ToString(nfi) + " " + ty1.ToString(nfi) +
					  " M " + x2.ToString(nfi) + " " + y2.ToString(nfi) +
					  " Q " + qx3.ToString(nfi) + " " + qy3.ToString(nfi) + " " + qx4.ToString(nfi) + " " + qy4.ToString(nfi) +
					  " T " + tx1.ToString(nfi) + " " + ty1.ToString(nfi));
		}

		public static List<Path> GetBracketsForAnnotations(List<SelectableTextBlock> annotations, List<SelectableTextBlock> tokens)
		{
			List<Path> output = new List<Path>();

			for (int i = 0; i < annotations.Count; i++)
			{
				var curToken = tokens[i];
				var curAnno = annotations[i];

				if (curAnno is null)
				{
					output.Add(null);
					continue;
				}

				//  Bracket
				var leftEdge = curToken.RenderTransform.Value.OffsetX;
				var bottomEdge = curToken.RenderTransform.Value.OffsetY;

				var bracketHeight = 5d;
				var bracketExpressivness = 0.6d;

				var bracket_Figures = PathFigureCollection.Parse(
					GetBracket(
						leftEdge + curToken.DesiredSize.Width,
						bottomEdge,
						leftEdge,
						bottomEdge,
						bracketHeight,
						bracketExpressivness
						));

				Path bracket_Path = new Path
				{
					Data = new PathGeometry()
					{
						Figures = bracket_Figures
					},
					Stroke = curAnno.Background is null ? Brushes.Black.Clone() : curAnno.Background.Clone(),
					StrokeThickness = AppConfig.Config.BracketThickness
				};
				bracket_Path.Stroke.Opacity = AppConfig.Config.BracketOpacity;

				output.Add(bracket_Path);
			}

			return output;
		}
	}
}
