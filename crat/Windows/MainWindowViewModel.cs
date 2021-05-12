using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CRAT.Model;
using CRAT.Infrastructure;
using CRAT.Infrastructure.File;
using CRAT.Infrastructure.ImportExport;

namespace CRAT.Windows
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			//  Create command bindings
			InputDataLoadFileCommand = new CustomCommand(InputDataLoadFile);

			NewFileCommand = new CustomCommand(NewFile);
			OpenFileCommand = new CustomCommand(OpenFile);
			SaveFileCommand = new CustomCommand(Save);
			SaveFileAsCommand = new CustomCommand(SaveAs);

			LoadConfigCommand = new CustomCommand(LoadConfig);
			WriteConfigCommand = new CustomCommand(WriteConfig);

			DrawCommand = new CustomCommand(DrawText);
			DrawTextRandomDataCommand = new CustomCommand(DrawTextRandomData);

			TemplatesManagerCommand = new CustomCommand(ShowTemplateManager);

			AddAnnotationCommand = new CustomCommand(AddAnnotation);
			RemoveSelectedItemCommand = new CustomCommand(RemoveSelectedItem);
			AddRelationCommand = new CustomCommand(AddRelation);

			//  Try loading templates from predefined paths
			PrepareTemplates(
				out ObservableCollection<AnnotationTemplate> annotationTemplates,
				out ObservableCollection<RelationTemplate> relationTemplates);

			//  Create new model
			ModelData = new ModelData(null, new StyleData(annotationTemplates, relationTemplates));
		}

		#region Fields
		private string _statusBar_Status = "Ready.";
		private string _statusBar_Path = "No path selected.";

		private string _sentenceDataFilePath = null;

		private bool _saveButtonEnabled = false;

		private string _sentence = "";

		private ModelData _modelData;

		private int _selectedTokenIndex = -1;
		private int _selectedAnnotationIndex = -1;
		private int _selectedRelationIndex = -1;

		private int _selectedRelationSourceAnnotationIndex = -1;
		private int _selectedRelationTargetAnnotationIndex = -1;

		private readonly string sentenceDataFileFilter = "Data file (*.dat)|*.dat";
		#endregion

		#region Properties

		public string StatusBar_Status { get => _statusBar_Status; set { _statusBar_Status = value; OnPropertyChanged(); } }
		public string StatusBar_Path { get => _statusBar_Path; set { _statusBar_Path = value; OnPropertyChanged(); } }

		private string SentenceDataFilePath
		{
			get => _sentenceDataFilePath;
			set { SaveButtonEnabled = !(value is null); StatusBar_Path = value is null ? "No path selected." : value; _sentenceDataFilePath = value; }
		}
		public bool SaveButtonEnabled { get => _saveButtonEnabled; set { _saveButtonEnabled = value; OnPropertyChanged(); } }

		public AnnotationTemplate SelectedAnnotationTemplate { get; set; }
		public RelationTemplate SelectedRelationTemplate { get; set; }

		public string Sentence { get => _sentence; set { _sentence = value; OnPropertyChanged(); } }
		public ModelData ModelData { get => _modelData; set { _modelData = value; OnPropertyChanged(); } }

		public int SelectedTokenIndex { get => _selectedTokenIndex; set { _selectedTokenIndex = value; OnPropertyChanged(); } }
		public int SelectedAnnotationIndex { get => _selectedAnnotationIndex; set { _selectedAnnotationIndex = value; OnPropertyChanged(); } }
		public int SelectedRelationIndex { get => _selectedRelationIndex; set { _selectedRelationIndex = value; OnPropertyChanged(); } }

		public int SelectedRelationSourceAnnotationIndex { get => _selectedRelationSourceAnnotationIndex; set { _selectedRelationSourceAnnotationIndex = value; OnPropertyChanged(); } }
		public int SelectedRelationTargetAnnotationIndex { get => _selectedRelationTargetAnnotationIndex; set { _selectedRelationTargetAnnotationIndex = value; OnPropertyChanged(); } }

		#endregion

		private void PrepareTemplates(
			out ObservableCollection<AnnotationTemplate> annotationTemplates,
			out ObservableCollection<RelationTemplate> relationTemplates)
		{
			annotationTemplates = new ObservableCollection<AnnotationTemplate>(TemplateImporter.ImportAnnotations(AppConfig.Config.AnnotationTemplateDefaultFilePath));
			if (annotationTemplates.Count == 0)
				annotationTemplates = new ObservableCollection<AnnotationTemplate>();
			else
				SelectedAnnotationTemplate = annotationTemplates[0];

			relationTemplates = new ObservableCollection<RelationTemplate>(TemplateImporter.ImportRelations(AppConfig.Config.RelationTemplateDefaultFilePath));
			if (relationTemplates.Count == 0)
				relationTemplates = new ObservableCollection<RelationTemplate>();
			else
				SelectedRelationTemplate = relationTemplates[0];
		}

		#region Public Commands - Model Altering

		#region Command Properties
		public ICommand AddAnnotationCommand { get; }
		public ICommand RemoveSelectedItemCommand { get; }
		public ICommand AddRelationCommand { get; }

		#endregion
		public void AddAnnotation()
		{
			if (SelectedTokenIndex == -1 || SelectedAnnotationTemplate == null)
			{
				StatusBar_Status = "Both an annotation template and a token have to be chosen.";
				return;
			}

			ModelData.SentenceData.AddAnnotation(SelectedAnnotationTemplate, SelectedTokenIndex);

			StatusBar_Status = "Annotation added.";
		}
		public void RemoveSelectedItem()
		{
			if (SelectedAnnotationIndex != -1 || SelectedTokenIndex != -1)
			{
				//	Remove an annotation
				if (SelectedAnnotationIndex != -1)
					ModelData.SentenceData.RemoveAnnotation(SelectedAnnotationIndex);
				else
					ModelData.SentenceData.RemoveAnnotation(SelectedTokenIndex);
				StatusBar_Status = "Annotation removed.";
			}
			else if (SelectedRelationIndex != -1)
			{
				//	Remove a relation
				ModelData.SentenceData.RemoveRelation(SelectedRelationIndex);
				StatusBar_Status = "Relation removed.";
			}
			else
			{
				//	Nothing is selected
				StatusBar_Status = "A relation or an annotation or it's tokens have to be selected to remove it.";
			}
		}

		public void AddRelation()
		{
			if (SelectedRelationTemplate == null)
				return;

			if (SelectedRelationSourceAnnotationIndex == -1 || SelectedRelationTargetAnnotationIndex == -1 || SelectedRelationSourceAnnotationIndex == SelectedRelationTargetAnnotationIndex)
			{
				StatusBar_Status = "A relation can only be drawn between two unique annotations.";
				return;
			}

			if (!ModelData.SentenceData.AddRelation(SelectedRelationTemplate, SelectedRelationSourceAnnotationIndex, SelectedRelationTargetAnnotationIndex))
			{
				MessageBox.Show("This relation cannot connect these annotations in this order.", "Invalid Model", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			SelectedRelationIndex = ModelData.SentenceData.Relations.Count - 1;
			StatusBar_Status = "Relation added.";
		}

		#endregion

		#region Public Commands - Menu

		#region Command Properties
		public ICommand InputDataLoadFileCommand { get; }

		public ICommand NewFileCommand { get; }
		public ICommand OpenFileCommand { get; }
		public ICommand SaveFileCommand { get; }
		public ICommand SaveFileAsCommand { get; }


		public ICommand LoadConfigCommand { get; }
		public ICommand WriteConfigCommand { get; }


		public ICommand DrawCommand { get; }
		public ICommand DrawTextRandomDataCommand { get; }


		public ICommand TemplatesManagerCommand { get; }

		#endregion

		#region Input Data
		public void InputDataLoadFile()
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "Text file (*.txt)|*.txt"
			};
			if (openFileDialog.ShowDialog() != true)
				return;

			var sentence = InputDataImporter.ImportInputData(openFileDialog.FileName);
			Sentence = sentence;
			ModelData = new ModelData(new SentenceData(sentence), ModelData.StyleData);

			StatusBar_Status = "Opened input file.";
		}
		#endregion

		#region Sentence Data
		public void NewFile()
		{
			var res = MessageBox.Show("Are you sure you want to create a new Sentence file?\nAny unsaved data will be lost."
				, "New file", MessageBoxButton.OKCancel, MessageBoxImage.Information);
			if (res != MessageBoxResult.OK)
				return;

			ModelData = new ModelData(new SentenceData(), ModelData.StyleData);
			SentenceDataFilePath = null;

			StatusBar_Status = "Ready.";
		}
		public void OpenFile()
		{
			var openFileDialog = new OpenFileDialog { Filter = sentenceDataFileFilter };
			if (openFileDialog.ShowDialog() != true)
				return;

			SentenceDataFilePath = openFileDialog.FileName;
			var sentence = SentenceImporter.ImportSentence(SentenceDataFilePath);
			ModelData = new ModelData(sentence, ModelData.StyleData);

			StatusBar_Status = "Opened sentence file.";
		}
		public void Save()
		{
			if (SentenceDataFilePath is null)
				return;
			SaveSentenceData();
		}
		public void SaveAs()
		{
			var saveFileDialog = new SaveFileDialog { Filter = sentenceDataFileFilter };
			if (saveFileDialog.ShowDialog() != true)
				return;

			SentenceDataFilePath = saveFileDialog.FileName;
			SaveSentenceData();
		}
		private void SaveSentenceData()
		{
			var exported = SentenceExporter.ExportSentence(ModelData.SentenceData);
			FileWriter.WriteFile(SentenceDataFilePath, exported);
			StatusBar_Status = "Saved.";
		}
		#endregion

		#region Style Data
		public void ShowTemplateManager()
		{
			var vm = new TemplatesManagerViewModel(ModelData.StyleData);

			var templateManager = new TemplatesManagerWindow
			{ DataContext = vm };
			templateManager.Show();

			if (!ModelValidator.ValidateRelationRestictions(ModelData))
			{
				var res = MessageBox.Show("Current sentence data is not supported by these relation restictions\nDo you want to continue?"
					, "Incorrect Model", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
				if (res != MessageBoxResult.Yes)
					ShowTemplateManager();
			}
		}
		#endregion

		#region Config
		public void LoadConfig()
		{
			if (ConfigImporter.LoadConfig())
				MessageBox.Show("Configuration loaded successfuly", "Configuration Load Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show("Configuration loading failed.", "Configuration Load Failed", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		public void WriteConfig()
		{
			if (ConfigExporter.WriteConfig())
				MessageBox.Show("Configuration exported successfuly", "Configuration Export Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show("Configuration export failed.", "Configuration Export Failed", MessageBoxButton.OK, MessageBoxImage.Information);

		}
		#endregion

		#region Draw
		private void DrawText()
		{
			ModelData = new ModelData(new SentenceData(Sentence), ModelData.StyleData);
			StatusBar_Status = "Ready.";
		}

		private void DrawTextRandomData()
		{
			Sentence = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse interdum sapien non nibh porttitor mollis vitae eget justo. Morbi sed.";
			ModelData = new ModelData(new SentenceData(Sentence), ModelData.StyleData);
			ModelData.GenerateRandomData();
			StatusBar_Status = "Ready.";
		}
		#endregion

		#region Export
		/*	Export functionality has been moved to MainWindows.xaml.cs */
		#endregion

		#endregion
	}
}
