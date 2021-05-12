using CRAT.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CRAT.Control
{
	public static class UIElementsArranger
	{
		private class AnnTok
		{
			public AnnTok(TextBlock a, TextBlock t) { Ann = a; Tok = t; }
			public TextBlock Ann { get; }
			public TextBlock Tok { get; }

			public double LeftOffset { get; set; } = 0d;
			public double RightOffset { get; set; } = 0d;

			public double Left { get; set; } = 0d;
			public double Middle => (Left + MaxWidth * 0.5);

			public double MaxWidth => (Ann is null || TokWidth > AnnWidth) ? TokWidth : AnnWidth;
			public double AnnWidth => Ann.DesiredSize.Width;
			public double TokWidth => Tok.DesiredSize.Width;
		}

		public static void ArrangeTokensAndAnnotations(
			double tokenVerticalOffset,
			List<SelectableTextBlock> tokens,
			List<SelectableTextBlock> annotations,
			Dictionary<Relation, List<UIElement>> relations)
		{
			//  Create AnnToks
			AnnTok[] annToks = new AnnTok[tokens.Count];
			for (int i = 0; i < tokens.Count; i++)
				annToks[i] = new AnnTok(annotations[i], tokens[i]);

			//  Initialize arrangement
			ArrangeAnnToks(annToks);

			//  Move AnnToks depending on Relations
			foreach (var item in relations)
			{
				var relation = item.Key;
				SelectableTextBlock naming = item.Value.Find(item => item is SelectableTextBlock) as SelectableTextBlock;

				var leftItem = annToks[relation.LeftIndex];
				var rightItem = annToks[relation.RightIndex];

				var relationMinWidth = naming.DesiredSize.Width + 15;
				var gap = Math.Abs((leftItem.Middle + leftItem.AnnWidth * 0.5) - (rightItem.Middle - rightItem.AnnWidth * 0.5));

				if (relationMinWidth > gap)
				{
					var diff = relationMinWidth - gap;
					var evenParts = diff / (relation.RightIndex - relation.LeftIndex);

					for (int i = relation.LeftIndex; i < relation.RightIndex; i++)
					{
						if (annToks[i].RightOffset < evenParts * 0.5)
							annToks[i].RightOffset = evenParts * 0.5;

						if (annToks[i + 1].LeftOffset < evenParts * 0.5)
							annToks[i + 1].LeftOffset = evenParts * 0.5;
					}
					ArrangeAnnToks(annToks);
				}
			}

			//  Finalize process
			foreach (var item in annToks)
			{
				var middleX = item.Middle;

				if (item.Ann != null)
					item.Ann.RenderTransform = new TranslateTransform
					{
						X = middleX - item.AnnWidth * 0.5,
						Y = tokenVerticalOffset - item.Ann.DesiredSize.Height - 5
					};

				item.Tok.RenderTransform = new TranslateTransform
				{
					X = middleX - item.TokWidth * 0.5,
					Y = tokenVerticalOffset
				};
			}
		}

		private static void ArrangeAnnToks(AnnTok[] annToks)
		{
			var left = AppConfig.Config.CanvasMargin;

			for (int i = 0; i < annToks.Length; i++)
			{
				var item = annToks[i];

				item.Left = left + item.LeftOffset;

				left = left + item.LeftOffset + item.MaxWidth + item.RightOffset + AppConfig.Config.TokensDefaultGap;
			}
		}

		public static double GetMaxHeight(Dictionary<Relation, List<UIElement>> relations, Dictionary<Relation, int> relationsLevelsDict, List<SelectableTextBlock> annotations, List<SelectableTextBlock> tokens)
		{
			var maxHeight = 0d;
			if (relations != null)
			{
				foreach (var pair in relations)
				{
					var item = pair.Key;
					var naming = pair.Value.Find(item => item is SelectableTextBlock);

					relationsLevelsDict.TryGetValue(item, out int level);

					var curHeight = level * AppConfig.Config.RelationsLevelGap + naming.DesiredSize.Height + AppConfig.Config.CanvasMargin + AppConfig.Config.FixedRelationOffset;
					if (maxHeight < curHeight)
						maxHeight = curHeight;
				}
			}

			if (annotations != null)
			{
				annotations.ForEach(item =>
				{
					if (item == null)
						return;
					var curHeight = item.DesiredSize.Height + AppConfig.Config.CanvasMargin;
					if (maxHeight < curHeight)
						maxHeight = curHeight;
				});
			}

			if (tokens != null)
			{
				tokens.ForEach(item =>
				{
					var curHeight = item.DesiredSize.Height + AppConfig.Config.CanvasMargin;
					if (maxHeight < curHeight)
						maxHeight = curHeight;
				});
			}

			return maxHeight;
		}
	}
}
