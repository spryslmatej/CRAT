using CRAT.Infrastructure.ImportExport;
using CRAT.Model;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CRAT.Control
{
	public class AnnotationCanvas : Canvas
	{
		#region Fields
		private List<SelectableTextBlock> _uiTokens;
		private List<SelectableTextBlock> _uiAnnotations;
		private List<List<UIElement>> _uiRelations;
		private List<Path> _uiBrackets;
		#endregion

		#region Dependency Properties

		#region Data Properties
		public static readonly DependencyProperty SentenceDataProperty = DependencyProperty.Register(nameof(SentenceData), typeof(SentenceData), typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(null, OnSentenceDataPropertyChanged));

		public static readonly DependencyProperty StyleDataProperty = DependencyProperty.Register(nameof(StyleData), typeof(StyleData), typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(null, OnStyleDataPropertyChanged));
		#endregion

		#region UIElements
		public static readonly DependencyProperty SelectedTokenIndexProperty = DependencyProperty.Register(
			nameof(SelectedTokenIndex),
			typeof(int),
			typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTokenIndexChanged));

		public static readonly DependencyProperty SelectedAnnotationIndexProperty = DependencyProperty.Register(
			nameof(SelectedAnnotationIndex),
			typeof(int),
			typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedAnnotationIndexChanged));

		public static readonly DependencyProperty SelectedRelationIndexProperty = DependencyProperty.Register(
			nameof(SelectedRelationIndex),
			typeof(int),
			typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedRelationIndexChanged));
		#endregion

		#region Commands
		public static readonly DependencyProperty SelectedDragAndDropSourceAnnotationIndexProperty = DependencyProperty.Register(
			nameof(SelectedDragAndDropSourceAnnotationIndex),
			typeof(int),
			typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDragAndDropSourceAnnotationIndexChanged));

		public static readonly DependencyProperty SelectedDragAndDropTargetAnnotationIndexProperty = DependencyProperty.Register(
			nameof(SelectedDragAndDropTargetAnnotationIndex),
			typeof(int),
			typeof(AnnotationCanvas),
			new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDragAndDropTargetAnnotationIndexChanged));


		public static readonly DependencyProperty TokenDoubleClickCommandProperty = DependencyProperty.Register(
			nameof(TokenDoubleClickCommand),
			typeof(ICommand),
			typeof(AnnotationCanvas));

		public static readonly DependencyProperty AnnotationsDragAndDropCommandProperty = DependencyProperty.Register(
			nameof(AnnotationsDragAndDropCommand),
			typeof(ICommand),
			typeof(AnnotationCanvas));
		#endregion

		#endregion

		#region Properties

		#region Datas
		public SentenceData SentenceData { get; set; }
		public StyleData StyleData { get; set; }
		#endregion

		#region Commands
		public ICommand TokenDoubleClickCommand
		{
			get => (ICommand)GetValue(TokenDoubleClickCommandProperty);
			set => SetValue(TokenDoubleClickCommandProperty, value);
		}
		public ICommand AnnotationsDragAndDropCommand
		{
			get => (ICommand)GetValue(AnnotationsDragAndDropCommandProperty);
			set => SetValue(AnnotationsDragAndDropCommandProperty, value);
		}
		#endregion

		#region Dependency Properties Changed
		private static void OnSelectedTokenIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvas = (AnnotationCanvas)d;
			var selectedTokenIndex = (int)e.OldValue;
			var newSelectedTokenIndex = (int)e.NewValue;

			if (selectedTokenIndex != -1) canvas.DeselectItem(canvas._uiTokens[selectedTokenIndex]);
			if (newSelectedTokenIndex != -1) canvas.SelectItem(canvas._uiTokens[newSelectedTokenIndex]);
		}
		private static void OnSelectedAnnotationIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvas = (AnnotationCanvas)d;
			var selectedAnnotationIndex = (int)e.OldValue;
			var newSelectedAnnotationIndex = (int)e.NewValue;

			if (selectedAnnotationIndex != -1) canvas.DeselectItem(canvas._uiAnnotations[selectedAnnotationIndex]);
			if (newSelectedAnnotationIndex != -1) canvas.SelectItem(canvas._uiAnnotations[newSelectedAnnotationIndex]);
		}
		private static void OnSelectedRelationIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvas = (AnnotationCanvas)d;
			var selectedRelationIndex = (int)e.OldValue;
			var newSelectedRelationIndex = (int)e.NewValue;

			if (selectedRelationIndex != -1)
			{
				var item = canvas._uiRelations[selectedRelationIndex].Find(item => item is SelectableTextBlock) as SelectableTextBlock;
				canvas.DeselectItem(item);
			}
			if (newSelectedRelationIndex != -1)
			{
				var newItem = canvas._uiRelations[newSelectedRelationIndex].Find(item => item is SelectableTextBlock) as SelectableTextBlock;
				canvas.SelectItem(newItem);
			}
		}

		private static void OnSelectedDragAndDropSourceAnnotationIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvas = (AnnotationCanvas)d;
			var oldIndex = (int)e.OldValue;
			var newIndex = (int)e.NewValue;

			if (oldIndex != -1) canvas.DeselectItem(canvas._uiAnnotations[oldIndex]);
			if (newIndex != -1) canvas.SelectItem(canvas._uiAnnotations[newIndex]);
		}
		private static void OnSelectedDragAndDropTargetAnnotationIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var canvas = (AnnotationCanvas)d;
			var oldIndex = (int)e.OldValue;
			var newIndex = (int)e.NewValue;

			if (oldIndex != -1) canvas.DeselectItem(canvas._uiAnnotations[oldIndex]);
			if (newIndex != -1) canvas.SelectItem(canvas._uiAnnotations[newIndex]);
		}
		#endregion

		#region Properties
		public int SelectedTokenIndex
		{
			get => (int)GetValue(SelectedTokenIndexProperty);
			set => SetValue(SelectedTokenIndexProperty, value);
		}
		public int SelectedAnnotationIndex
		{
			get => (int)GetValue(SelectedAnnotationIndexProperty);
			set => SetValue(SelectedAnnotationIndexProperty, value);
		}
		public int SelectedRelationIndex
		{
			get => (int)GetValue(SelectedRelationIndexProperty);
			set => SetValue(SelectedRelationIndexProperty, value);
		}

		public int SelectedDragAndDropSourceAnnotationIndex
		{
			get => (int)GetValue(SelectedDragAndDropSourceAnnotationIndexProperty);
			set => SetValue(SelectedDragAndDropSourceAnnotationIndexProperty, value);
		}
		public int SelectedDragAndDropTargetAnnotationIndex
		{
			get => (int)GetValue(SelectedDragAndDropTargetAnnotationIndexProperty);
			set => SetValue(SelectedDragAndDropTargetAnnotationIndexProperty, value);
		}
		#endregion

		#endregion

		#region Export
		public List<string> VisitorExport(AbstractCanvasExporter exporter)
		{
			return exporter.Export(_uiTokens, _uiAnnotations, _uiRelations, _uiBrackets, DesiredSize);
		}
		#endregion

		#region Mouse Event Handling
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			SelectedDragAndDropSourceAnnotationIndex = SelectedAnnotationIndex;
			Click_Lmb(e);
			SelectedDragAndDropTargetAnnotationIndex = SelectedAnnotationIndex;

			if (AnnotationsDragAndDropCommand != null)
				AnnotationsDragAndDropCommand.Execute(null);

			e.Handled = true;
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2 && TokenDoubleClickCommand != null)
				TokenDoubleClickCommand.Execute(null);

			Click_Lmb(e);
			e.Handled = true;
		}

		private void Click_Lmb(MouseButtonEventArgs e)
		{
			//  Reset selected indexes which resets background automatically through setters
			ResetSelectedIndexes();

			//  Find if the item was a token, an annotation, or a relation
			if (e.OriginalSource is SelectableTextBlock selectedItem)
			{
				var index = _uiTokens.FindIndex(item => item == selectedItem);
				if (index != -1) { SelectedTokenIndex = index; return; }

				index = _uiAnnotations.FindIndex(item => item == selectedItem);
				if (index != -1) { SelectedAnnotationIndex = index; return; }

				index = _uiRelations.FindIndex(item => item.Find(e => e is SelectableTextBlock) == selectedItem);
				if (index != -1) { SelectedRelationIndex = index; }
			}
		}
		#endregion

		#region PropertyChanged Functionality
		private static void OnSentenceDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AnnotationCanvas canvas = (AnnotationCanvas)d;
			canvas.SentenceData = e.NewValue as SentenceData;

			var newData = e.NewValue as SentenceData;

			if (e.OldValue != null)
			{
				canvas.RemoveListeners((SentenceData)e.OldValue);
			}

			if (e.NewValue != null)
			{
				canvas.RegisterListeners((SentenceData)e.NewValue);

				canvas.Refresh(newData);
			}
		}
		private static void OnStyleDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AnnotationCanvas canvas = (AnnotationCanvas)d;
			canvas.StyleData = e.NewValue as StyleData;

			var newData = e.NewValue as StyleData;

			if (e.OldValue != null)
			{
				canvas.RemoveListeners((StyleData)e.OldValue);
			}

			if (e.NewValue != null)
			{
				canvas.RegisterListeners((StyleData)e.NewValue);

				canvas.Refresh(newData);
			}
		}
		private void RegisterListeners(SentenceData data)
		{
			if (data != null)
			{
				//  Tokens do not change
				data.Annotations.CollectionChanged += OnSentenceDataItemsChange;
				data.Relations.CollectionChanged += OnSentenceDataItemsChange;
			}
		}
		private void RegisterListeners(StyleData data)
		{
			if (data != null)
			{
				data.AnnotationTemplates.CollectionChanged += OnStyleDataItemsChange;
				data.RelationTemplates.CollectionChanged += OnStyleDataItemsChange;
			}
		}
		private void RemoveListeners(SentenceData data)
		{
			if (data != null)
			{
				data.Annotations.CollectionChanged -= OnSentenceDataItemsChange;
				data.Relations.CollectionChanged -= OnSentenceDataItemsChange;
			}
		}
		private void RemoveListeners(StyleData data)
		{
			if (data != null)
			{
				data.AnnotationTemplates.CollectionChanged -= OnStyleDataItemsChange;
				data.RelationTemplates.CollectionChanged -= OnStyleDataItemsChange;
			}
		}
		private void OnSentenceDataItemsChange(object sender, NotifyCollectionChangedEventArgs e) { Refresh(SentenceData); }
		private void OnStyleDataItemsChange(object sender, NotifyCollectionChangedEventArgs e) { Refresh(StyleData); }
		#endregion

		#region Private Methods -- Drawing
		private void Refresh(SentenceData data)
		{
			if (data is null || StyleData is null)
				return;

			//	Reset selected items
			ResetSelectedIndexes();

			//	Clear the canvas
			Children.Clear();

			//  Check if model is valid
			if (!ModelValidator.ValidateSentenceData(data))
			{
				MessageBox.Show("Invalid model cannot be drawn.", "Invalid Model", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			//	Load UIElements
			LoadUIElements(data);

			//	Clear canvas
			Children.Clear();

			//	Refresh SelectableTextBlock styles
			RefreshUIElementsStyle(StyleData);

			//  Draw on canvas
			AddElementsAsChildren();

			//  Refresh canvas
			RefreshVisual();
		}
		private void Refresh(StyleData data)
		{
			if (SentenceData is null || data is null ||
				_uiAnnotations is null)
				return;

			//	Reset selected items
			ResetSelectedIndexes();

			//	Clear canvas
			Children.Clear();

			//	Refresh SelectableTextBlock styles
			RefreshUIElementsStyle(data);

			//  Draw on canvas
			AddElementsAsChildren();

			//  Refresh canvas
			RefreshVisual();
		}

		private void ResetSelectedIndexes()
		{
			SelectedTokenIndex = -1;
			SelectedAnnotationIndex = -1;
			SelectedRelationIndex = -1;
		}
		private void LoadUIElements(SentenceData data)
		{
			//  Analyze Relations to get their levels
			Dictionary<Relation, int> relationsLevelsDict = RelationsAnalyzer.AsignLevelsToRelations(new List<Relation>(data.Relations));

			//  Get respective UIElements for model items
			UIElementsFactory.TokensAndAnnotationsToTextBlock(
				data,
				out List<SelectableTextBlock> uitokens,
				out List<SelectableTextBlock> uianno);
			_uiTokens = uitokens;
			_uiAnnotations = uianno;

			//  Initialize relation to UIElements dictionary
			Dictionary<Relation, List<UIElement>> _relationsToUIElements = new Dictionary<Relation, List<UIElement>>();
			foreach (var r in data.Relations)
				_relationsToUIElements.Add(r, new List<UIElement>());

			//  Add namings to Relations dictionary
			UIElementsFactory.AddNamingsToRelationDictionary(_relationsToUIElements);

			//  Get max vertical offset for correct arrangement
			var tokenVerticalOffset = UIElementsArranger.GetMaxHeight(_relationsToUIElements, relationsLevelsDict, _uiAnnotations, _uiTokens);

			//  Arrange UIElements
			UIElementsArranger.ArrangeTokensAndAnnotations(tokenVerticalOffset, _uiTokens, _uiAnnotations, _relationsToUIElements);

			//  Get brackets for Annotations
			_uiBrackets = UIElementsFactory.GetBracketsForAnnotations(_uiAnnotations, _uiTokens);

			//  Add rest of relation UIElements to dictionary
			UIElementsFactory.AddResemblanceToRelationsToUIElements(_relationsToUIElements, relationsLevelsDict, _uiAnnotations);

			//  Asign items to UIRelations
			_uiRelations = new List<List<UIElement>>(_relationsToUIElements.Values);
		}
		private void RefreshUIElementsStyle(StyleData data)
		{
			//  Initialize dictionary
			Dictionary<string, AnnotationTemplate> templatesDictionary = new Dictionary<string, AnnotationTemplate>();
			foreach (AnnotationTemplate item in data.AnnotationTemplates)
				templatesDictionary.Add(item.Text, item);

			for (int i = 0; i < _uiAnnotations.Count; i++)
			{
				var item = _uiAnnotations[i];
				var token = _uiTokens[i];
				var bracket = _uiBrackets[i];

				if (item is null)
					continue;

				if (templatesDictionary.TryGetValue(item.Text, out AnnotationTemplate t))
				{
					var background = t?.Brush?.Clone();

					if (!(background is null))
					{
						item.DefaultBackground = background.Clone();
						token.DefaultBackground = background.Clone();
						bracket.Stroke = background.Clone();

						item.DefaultBackground.Opacity = AppConfig.Config.BackgroundOpacity_Annotations;
						token.DefaultBackground.Opacity = AppConfig.Config.BackgroundOpacity_Tokens;
						bracket.Stroke.Opacity = AppConfig.Config.BracketOpacity;

						item.ResetBackground();
						token.ResetBackground();

						continue;
					}
				}

				item.DefaultBackground = null;
				token.DefaultBackground = null;
				bracket.Stroke = Brushes.Black.Clone();

				item.ResetBackground();
				token.ResetBackground();
			}
		}
		#endregion

		#region Private Methods -- Item Selection
		private void SelectItem(SelectableTextBlock item)
		{
			item.Background = (Brush)new BrushConverter().ConvertFromString(AppConfig.Config.SelectedTextBlock_Color);
			item.Background.Opacity = AppConfig.Config.SelectedTextBlock_Opacity;
		}
		private void DeselectItem(SelectableTextBlock item)
		{
			item.ResetBackground();
		}
		#endregion

		#region Private Methods -- General Visual

		private void RefreshVisual() { Dispatcher.Invoke(delegate () { }, DispatcherPriority.Render); }

		protected override Size MeasureOverride(Size constraint)
		{
			base.MeasureOverride(constraint);

			// isEmpty?
			double width = 0d, height = 0d;
			if (_uiTokens != null && _uiTokens.Count != 0)
			{
				var lastToken = _uiTokens[_uiTokens.Count - 1];
				width = lastToken.RenderTransform.Value.OffsetX + lastToken.DesiredSize.Width + AppConfig.Config.CanvasMargin;
				height = lastToken.RenderTransform.Value.OffsetY + lastToken.DesiredSize.Height;
			}

			if (_uiAnnotations != null && _uiAnnotations.Count != 0)
			{
				var lastAnno = _uiAnnotations[_uiAnnotations.Count - 1];
				if (lastAnno != null)
				{
					var curWidth = lastAnno.RenderTransform.Value.OffsetX + lastAnno.DesiredSize.Width + AppConfig.Config.CanvasMargin;
					if (width < curWidth)
						width = curWidth;

					var curHeight = lastAnno.RenderTransform.Value.OffsetY + lastAnno.DesiredSize.Height;
					if (height < curHeight)
						height = curHeight;
				}
			}

			if (_uiRelations != null)
			{
				foreach (var relation in _uiRelations)
				{
					foreach (var item in relation)
					{
						var curHeight = item.RenderTransform.Value.OffsetY + item.DesiredSize.Height;
						if (height < curHeight)
							height = curHeight;
					}
				}
			}

			return new Size(width, height);
		}

		#endregion

		#region Private Methods -- UI Elements

		private void AddTokens()
		{
			if (_uiTokens is null)
				return;

			foreach (var t in _uiTokens)
				Children.Add(t);
		}
		private void AddAnnotations()
		{
			if (_uiAnnotations is null)
				return;

			foreach (var a in _uiAnnotations)
			{
				if (a != null)
					Children.Add(a);
			}
		}
		private void AddRelations()
		{
			if (_uiRelations is null)
				return;

			foreach (var item in _uiRelations)
			{
				foreach (var element in item)
					Children.Add(element);
			}
		}
		private void AddBrackets()
		{
			if (_uiBrackets is null)
				return;

			foreach (var item in _uiBrackets)
			{
				if (item != null)
					Children.Add(item);
			}
		}

		private void AddElementsAsChildren()
		{
			//  Tokens
			AddTokens();

			//  Annotations
			AddAnnotations();

			//  Relations
			AddRelations();

			//  Brackets
			AddBrackets();
		}

		#endregion

	}
}
