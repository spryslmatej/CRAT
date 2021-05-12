using System.Collections.ObjectModel;

namespace CRAT.Model
{
	/// <summary>
	/// Class for containing annotation and relation templates
	/// </summary>
	public class StyleData
	{
		public ObservableCollection<AnnotationTemplate> AnnotationTemplates { get; }
		public ObservableCollection<RelationTemplate> RelationTemplates { get; }

		public StyleData(ObservableCollection<AnnotationTemplate> at = null, ObservableCollection<RelationTemplate> rt = null)
		{
			if (at is null)
				at = new ObservableCollection<AnnotationTemplate>();
			if (rt is null)
				rt = new ObservableCollection<RelationTemplate>();

			AnnotationTemplates = at;
			RelationTemplates = rt;
		}

		#region Public Methods -- Properties Modification
		public bool AddAnnotationTemplate(AnnotationTemplate at)
		{
			if (AnnotationTemplates.Contains(at))
				return false;

			AnnotationTemplates.Add(at);
			return true;
		}
		public bool RemoveAnnotationTemplate(AnnotationTemplate at)
		{
			if (AnnotationTemplates.Remove(at))
			{
				foreach (var item in RelationTemplates)
				{
					if (item.SourceAnnotations.Contains(at.Text))
						item.SourceAnnotations.Remove(at.Text);
					if (item.DestinationAnnotations.Contains(at.Text))
						item.DestinationAnnotations.Remove(at.Text);
				}
				return true;
			}
			return false;
		}

		public bool AddRelationTemplate(RelationTemplate rt)
		{
			if (RelationTemplates.Contains(rt))
				return false;

			RelationTemplates.Add(rt);
			return true;
		}
		public bool RemoveRelationTemplate(RelationTemplate rt)
		{
			return RelationTemplates.Remove(rt);
		}
		#endregion
	}
}
