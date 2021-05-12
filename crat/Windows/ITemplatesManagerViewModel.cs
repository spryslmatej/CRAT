using CRAT.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace CRAT.Windows
{
	public interface ITemplatesManagerViewModel
	{
		ICommand AddAnnotationTemplateCommand { get; }
		ICommand AddDestinationItemToRelationCommand { get; }
		ICommand AddRelationTemplateCommand { get; }
		ICommand AddSourceItemToRelationCommand { get; }
		string AnnotationName { get; set; }
		ColorItem Color { get; set; }
		ICommand DeleteDestinationItemFromRelationCommand { get; }
		ICommand DeleteSourceItemFromRelationCommand { get; }
		ICommand ExportAnnotationsCommand { get; }
		ICommand ExportRelationsCommand { get; }
		ICommand ImportAnnotationsCommand { get; }
		ICommand ImportRelationsCommand { get; }
		string RelationName { get; set; }
		ICommand RemoveAnnotationTemplateCommand { get; }
		ICommand RemoveRelationTemplateCommand { get; }
		AnnotationTemplate SelectedAnnotationTemplate { get; set; }
		string SelectedDestItem { get; set; }
		RelationTemplate SelectedRelationTemplate { get; set; }
		string SelectedSourceItem { get; set; }
		StyleData StyleData { get; set; }

		event PropertyChangedEventHandler PropertyChanged;

		void AddAnnotation();
		void AddDestinationItemToRelation();
		void AddRelation();
		void AddSourceItemToRelation();
		void DeleteDestinationItemFromRelation();
		void DeleteSourceItemFromRelation();
		void ExportAnnotations();
		void ExportRelations();
		void ImportAnnotations();
		void ImportRelations();
		void RemoveAnnotation();
		void RemoveRelation();
	}
}