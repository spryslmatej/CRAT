using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using CRAT.Model;
using CRAT.Infrastructure;
using CRAT.Infrastructure.File;
using CRAT.Infrastructure.ImportExport;

namespace CRAT.Windows
{
	public class TemplatesManagerViewModel : BaseViewModel, ITemplatesManagerViewModel
	{
		public TemplatesManagerViewModel(
			StyleData sd)
		{
			if (sd is null)
				throw new ArgumentException("Arguments cannot be null.");

			StyleData = sd;

			//  Annotations
			AddAnnotationTemplateCommand = new CustomCommand(AddAnnotation);
			RemoveAnnotationTemplateCommand = new CustomCommand(RemoveAnnotation);

			ExportAnnotationsCommand = new CustomCommand(ExportAnnotations);
			ImportAnnotationsCommand = new CustomCommand(ImportAnnotations);

			//  Relations
			AddRelationTemplateCommand = new CustomCommand(AddRelation);
			RemoveRelationTemplateCommand = new CustomCommand(RemoveRelation);

			AddSourceItemToRelationCommand = new CustomCommand(AddSourceItemToRelation);
			DeleteSourceItemFromRelationCommand = new CustomCommand(DeleteSourceItemFromRelation);

			AddDestinationItemToRelationCommand = new CustomCommand(AddDestinationItemToRelation);
			DeleteDestinationItemFromRelationCommand = new CustomCommand(DeleteDestinationItemFromRelation);

			ExportRelationsCommand = new CustomCommand(ExportRelations);
			ImportRelationsCommand = new CustomCommand(ImportRelations);
		}

		#region Commands

		#region Annotations
		public ICommand AddAnnotationTemplateCommand { get; }
		public ICommand RemoveAnnotationTemplateCommand { get; }
		public ICommand ExportAnnotationsCommand { get; }
		public ICommand ImportAnnotationsCommand { get; }
		#endregion

		#region Relations
		public ICommand AddRelationTemplateCommand { get; }
		public ICommand RemoveRelationTemplateCommand { get; }
		public ICommand AddSourceItemToRelationCommand { get; }
		public ICommand DeleteSourceItemFromRelationCommand { get; }
		public ICommand AddDestinationItemToRelationCommand { get; }
		public ICommand DeleteDestinationItemFromRelationCommand { get; }
		public ICommand ExportRelationsCommand { get; }
		public ICommand ImportRelationsCommand { get; }
		#endregion

		#endregion

		#region Fields
		private AnnotationTemplate _selectedAnnotationTemplate;
		private RelationTemplate _selectedRelationTemplate;
		private readonly string fileDialogFilter = "Data file (*.dat)|*.dat";
		#endregion

		#region Properties
		public StyleData StyleData { get; set; }

		public AnnotationTemplate SelectedAnnotationTemplate
		{
			get => _selectedAnnotationTemplate;
			set { _selectedAnnotationTemplate = value; OnPropertyChanged(); }
		}
		public RelationTemplate SelectedRelationTemplate
		{
			get => _selectedRelationTemplate;
			set { _selectedRelationTemplate = value; OnPropertyChanged(); }
		}
		public string AnnotationName { get; set; }
		public ColorItem Color { get; set; }
		public string RelationName { get; set; }
		public string SelectedSourceItem { get; set; }
		public string SelectedDestItem { get; set; }
		#endregion

		#region Annotation Templates
		public void AddAnnotation()
		{
			if (string.IsNullOrEmpty(AnnotationName) || Color is null)
				return;

			var newTemplate = new AnnotationTemplate(AnnotationName, Color.Brush);
			if (!StyleData.AddAnnotationTemplate(newTemplate))
				MessageBox.Show("Duplicates are not allowed.", "", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				SelectedAnnotationTemplate = newTemplate;
		}
		public void RemoveAnnotation()
		{
			if (SelectedAnnotationTemplate is null)
				return;

			StyleData.RemoveAnnotationTemplate(SelectedAnnotationTemplate);
			if (StyleData.AnnotationTemplates.Count != 0)
				SelectedAnnotationTemplate = StyleData.AnnotationTemplates[StyleData.AnnotationTemplates.Count - 1];
		}
		public void ExportAnnotations()
		{
			var saveFileDialog = new SaveFileDialog { Filter = fileDialogFilter };
			if (saveFileDialog.ShowDialog() != true)
				return;

			var exported = TemplateExporter.ExportAnnotations(StyleData.AnnotationTemplates);
			FileWriter.WriteFile(saveFileDialog.FileName, exported);
		}
		public void ImportAnnotations()
		{
			var openFileDialog = new OpenFileDialog { Filter = fileDialogFilter };
			if (openFileDialog.ShowDialog() != true)
				return;

			var imported = TemplateImporter.ImportAnnotations(openFileDialog.FileName);

			if (imported.Count == 0)
			{
				MessageBox.Show("This file does not contain the definitions for annotations.", "File failed to load", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			StyleData.AnnotationTemplates.Clear();
			imported.ForEach(item => StyleData.AddAnnotationTemplate(item));

			if (StyleData.AnnotationTemplates.Count == 0)
				MessageBox.Show("Templates file was empty!", "File loaded", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		#endregion

		#region Relation Templates
		public void AddRelation()
		{
			if (string.IsNullOrEmpty(RelationName))
				return;
			var newTemplate = new RelationTemplate(RelationName, null, null);

			if (!StyleData.AddRelationTemplate(newTemplate))
				MessageBox.Show("Duplicates are not allowed.", "", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				SelectedRelationTemplate = newTemplate;
		}
		public void RemoveRelation()
		{
			if (SelectedRelationTemplate is null)
				return;
			StyleData.RemoveRelationTemplate(SelectedRelationTemplate);

			if (StyleData.RelationTemplates.Count != 0)
				SelectedRelationTemplate = StyleData.RelationTemplates[StyleData.RelationTemplates.Count - 1];
		}

		public void AddSourceItemToRelation() { SelectedRelationTemplate.AddSourceAnnotation(SelectedAnnotationTemplate.Text); }
		public void DeleteSourceItemFromRelation() { SelectedRelationTemplate.RemoveSourceAnnotation(SelectedSourceItem); }
		public void AddDestinationItemToRelation() { SelectedRelationTemplate.AddDestinationAnnotation(SelectedAnnotationTemplate.Text); }
		public void DeleteDestinationItemFromRelation() { SelectedRelationTemplate.RemoveDestinationAnnotation(SelectedDestItem); }

		public void ExportRelations()
		{
			var saveFileDialog = new SaveFileDialog { Filter = fileDialogFilter };
			if (saveFileDialog.ShowDialog() != true)
				return;

			var exported = TemplateExporter.ExportRelations(StyleData.RelationTemplates);
			FileWriter.WriteFile(saveFileDialog.FileName, exported);
		}
		public void ImportRelations()
		{
			var openFileDialog = new OpenFileDialog { Filter = fileDialogFilter };
			if (openFileDialog.ShowDialog() != true)
				return;

			var imported = TemplateImporter.ImportRelations(openFileDialog.FileName);

			if (imported.Count == 0)
			{
				MessageBox.Show("This file does not contain the definitions for relations.", "File failed to load", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			StyleData.RelationTemplates.Clear();
			imported.ForEach(item => StyleData.AddRelationTemplate(item));

			if (StyleData.RelationTemplates.Count == 0)
				MessageBox.Show("Templates file was empty.", "File loaded", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		#endregion
	}
}
