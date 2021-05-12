using System.Collections.Generic;

namespace CRAT.Model
{
	public static class ModelValidator
	{
		public static bool ValidateSentenceData(SentenceData data)
		{
			bool success = true;

			//  Annotations
			foreach (Annotation a in data.Annotations)
			{
				//  Check if annotation's token index is out of bounds
				if (a != null &&
					(a.TokenIndex < 0 || data.Tokens.Count <= a.TokenIndex ||
					data.Tokens[a.TokenIndex] == null))
					return false;
			}

			//  Relations
			foreach (Relation r in data.Relations)
			{
				//  Check if relation indexes are out of bounds and if these Annotations exist
				if (r.LeftIndex < 0 || data.Annotations.Count <= r.LeftIndex ||
					r.RightIndex < 0 || data.Annotations.Count <= r.RightIndex ||
					data.Annotations[r.LeftIndex] == null ||
					data.Annotations[r.RightIndex] == null)
					return false;
			}

			return success;
		}

		public static bool ValidateRelationRestictions(ModelData data)
		{
			bool success = true;

			Dictionary<string, RelationTemplate> pairs = new Dictionary<string, RelationTemplate>();
			foreach (var item in data.StyleData.RelationTemplates)
				pairs.Add(item.Text, item);

			foreach (var relation in data.SentenceData.Relations)
			{
				if (pairs.TryGetValue(relation.Text, out RelationTemplate template))
				{
					var source = data.SentenceData.Annotations[relation.GetSourceIndex()];
					var dest = data.SentenceData.Annotations[relation.GetDestinationIndex()];
					if (
						(template.SourceAnnotations.Count != 0 && !template.SourceAnnotations.Contains(source.Text)) ||
						(template.DestinationAnnotations.Count != 0 && !template.DestinationAnnotations.Contains(dest.Text))
						)
						return false;
				}
			}

			return success;
		}
	}
}
