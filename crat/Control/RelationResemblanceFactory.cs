using CRAT.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CRAT.Control
{
	public static class RelationResemblanceFactory
	{
		private readonly struct Edges
		{
			public Edges(SelectableTextBlock leftAnnotation, SelectableTextBlock rightAnnotation, int level)
			{
				LeftWidth = leftAnnotation.DesiredSize.Width;
				RightWidth = rightAnnotation.DesiredSize.Width;

				LeftEdge = leftAnnotation.RenderTransform.Value.OffsetX + LeftWidth;    //End of left annotation
				RightEdge = rightAnnotation.RenderTransform.Value.OffsetX;    //Start of right annotation

				BottomEdge = leftAnnotation.RenderTransform.Value.OffsetY + leftAnnotation.DesiredSize.Height * 0.5;   //Middle of Annotations
				TopEdge = BottomEdge - (level * AppConfig.Config.RelationsLevelGap) - AppConfig.Config.FixedRelationOffset;
			}

			public double LeftEdge { get; }
			public double RightEdge { get; }
			public double TopEdge { get; }
			public double BottomEdge { get; }
			public double LeftWidth { get; }
			public double RightWidth { get; }

		};

		private static Path GetConnectionPath(
			Edges edges,
			double leftAnnotationHeight,
			double rightAnnotationHeight,
			bool leftToRight)
		{
			NumberFormatInfo nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };

			var leftEdge = edges.LeftEdge;
			var rightEdge = edges.RightEdge;
			var topEdge = edges.TopEdge;
			var bottomEdge = edges.BottomEdge;

			//	Move the endpoint a bit further for readability
			if (leftToRight) { rightEdge += AppConfig.Config.RelationEndPointMove; }
			else { leftEdge -= AppConfig.Config.RelationEndPointMove; }

			var curveXlength = 30d;
			if (rightEdge - leftEdge < 2 * curveXlength)
				curveXlength = (rightEdge - leftEdge) * 0.5;

			var curveControlPointOffset = 1d;

			Point p1, p2, p3, p4, p5, p6;
			if (leftToRight)
			{
				//  Start
				p1 = new Point(leftEdge, bottomEdge);
				//  Q Curve control point
				p2 = new Point((leftEdge + curveControlPointOffset), (topEdge));
				//  Q Curve end point
				p3 = new Point(leftEdge + curveXlength, topEdge);
				//  Straight line end point
				p4 = new Point(rightEdge - curveXlength, topEdge);
				//  Q Curve control point
				p5 = new Point((rightEdge - curveControlPointOffset), (topEdge));
				//  Q Curve end point
				p6 = new Point(rightEdge, bottomEdge - rightAnnotationHeight * 0.5);
			}
			else
			{
				//  Start
				p1 = new Point(rightEdge, bottomEdge);
				//  Q Curve control point
				p2 = new Point((rightEdge - curveControlPointOffset), (topEdge));
				//  Q Curve end point
				p3 = new Point(rightEdge - curveXlength, topEdge);
				//  Straight line end point
				p4 = new Point(leftEdge + curveXlength, topEdge);
				//  Q Curve control point
				p5 = new Point((leftEdge + curveControlPointOffset), (topEdge));
				//  Q Curve end point
				p6 = new Point(leftEdge, bottomEdge - leftAnnotationHeight * 0.5);
			}

			var figures = PathFigureCollection.Parse($@"M{p1.X.ToString(nfi)},{(p1.Y.ToString(nfi))
				} Q{p2.X.ToString(nfi)},{p2.Y.ToString(nfi)
				}  {p3.X.ToString(nfi)},{p3.Y.ToString(nfi)
				} L{p4.X.ToString(nfi)},{p4.Y.ToString(nfi)
				} Q{p5.X.ToString(nfi)},{p5.Y.ToString(nfi)
				}  {p6.X.ToString(nfi)},{p6.Y.ToString(nfi)
					}");

			Path mypath = new Path
			{
				Data = new PathGeometry() { Figures = figures },
				Stroke = new BrushConverter().ConvertFromString(AppConfig.Config.RelationLineColor) as Brush,
				StrokeThickness = AppConfig.Config.RelationLineThickness
			};

			return mypath;
		}

		private static SelectableTextBlock GetTransformedNaming(
			Edges edges,
			SelectableTextBlock naming)
		{
			var leftEdge = edges.LeftEdge;
			var rightEdge = edges.RightEdge;
			var topEdge = edges.TopEdge;

			naming.RenderTransform = new TranslateTransform
			{
				X = leftEdge + 0.5 * (rightEdge - leftEdge) - naming.DesiredSize.Width * 0.5,
				Y = topEdge - naming.DesiredSize.Height * 0.5
			};

			return naming;
		}

		private static Polygon GetArrow(
			Edges edges,
			int level,
			double leftAnnotationHeight,
			double rightAnnotationHeight,
			bool leftToRight)
		{
			var leftEdge = edges.LeftEdge;
			var rightEdge = edges.RightEdge;
			var bottomEdge = edges.BottomEdge;

			//	Move the endpoint a bit further for readability
			if (leftToRight) { rightEdge += AppConfig.Config.RelationEndPointMove; }
			else { leftEdge -= AppConfig.Config.RelationEndPointMove; }

			var arrow = new Polygon
			{
				Stroke = Brushes.Black,
				Fill = Brushes.Black
			};
			arrow.Points.Add(new Point(-2.6, -3));      // *
			arrow.Points.Add(new Point(-2.6, 3));       //     *  (an equilateral triangle)
			arrow.Points.Add(new Point(2.6, 0));        // *

			var transformations = new TransformGroup();

			if (level != 0)
				transformations.Children.Add(new RotateTransform(-45));

			if (leftToRight)
			{
				transformations.Children.Add(new TranslateTransform
				{
					X = rightEdge,
					Y = bottomEdge - rightAnnotationHeight * 0.5
				});
			}
			else
			{
				transformations.Children.Add(new RotateTransform(180));

				transformations.Children.Add(new TranslateTransform
				{
					X = leftEdge,
					Y = bottomEdge - leftAnnotationHeight * 0.5
				});
			}

			arrow.RenderTransform = transformations;

			return arrow;
		}

		public static List<UIElement> RelationToUIElements(Relation relation, int level, SelectableTextBlock naming, SelectableTextBlock leftAnnotation, SelectableTextBlock rightAnnotation)
		{
			Edges edges = new Edges(leftAnnotation, rightAnnotation, level);

			List<UIElement> output = new List<UIElement>
			{
                /*
                 *  Connection                    
                 */
                GetConnectionPath(
					edges,
				leftAnnotation.DesiredSize.Height, rightAnnotation.DesiredSize.Height,
				relation.LeftToRightFlow),

                /*  
                 *  Naming
                 */
                GetTransformedNaming(edges, naming),

                /*  
                 *  Arrow
                 */
                GetArrow(
				edges,
				level,
				leftAnnotation.DesiredSize.Height, rightAnnotation.DesiredSize.Height,
				relation.LeftToRightFlow)
			};

			return output;
		}
	}
}
