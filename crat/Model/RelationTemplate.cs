using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace CRAT.Model
{
	public sealed class RelationTemplate : IEquatable<RelationTemplate>
	{
		public string Text { get; }

		//  Used as a whitelist; Empty == everything is allowed
		public ObservableCollection<string> SourceAnnotations { get; }
		public ObservableCollection<string> DestinationAnnotations { get; }


		public RelationTemplate(string t, List<string> sa = null, List<string> da = null)
		{
			var noWhitespaces = Regex.Replace(t, @"\s+", "");
			if (noWhitespaces.Length == 0)
				throw new ArgumentException("Annotation text cannot be empty.");

			if (sa is null)
				sa = new List<string>();
			if (da is null)
				da = new List<string>();

			SourceAnnotations = new ObservableCollection<string>(sa);
			DestinationAnnotations = new ObservableCollection<string>(da);

			Text = t;
		}

		public bool AddSourceAnnotation(string anno)
		{
			if (string.IsNullOrEmpty(anno)) return false;
			if (SourceAnnotations.Contains(anno)) return false;
			SourceAnnotations.Add(anno);
			return true;
		}
		public bool AddDestinationAnnotation(string anno)
		{
			if (string.IsNullOrEmpty(anno)) return false;
			if (DestinationAnnotations.Contains(anno)) return false;
			DestinationAnnotations.Add(anno);
			return true;
		}
		public bool RemoveSourceAnnotation(string anno) { return SourceAnnotations.Remove(anno); }
		public bool RemoveDestinationAnnotation(string anno) { return DestinationAnnotations.Remove(anno); }

		public override string ToString()
		{
			StringBuilder output = new StringBuilder();
			output.Append(Text);
			output.Append(";");

			foreach (var item in SourceAnnotations)
				output.Append(item + " ");

			output.Append(";");
			foreach (var item in DestinationAnnotations)
				output.Append(item + " ");

			return output.ToString();
		}
		public static RelationTemplate FromString(string s)
		{
			var properties = s.Split(";");

			List<string> source = new List<string>();
			var srcSplit = properties[1].Split(" ");
			foreach (var item in srcSplit)
				if (!string.IsNullOrEmpty(item)) source.Add(item);

			List<string> dest = new List<string>();
			var destSplit = properties[2].Split(" ");
			foreach (var item in destSplit)
				if (!string.IsNullOrEmpty(item)) dest.Add(item);

			return new RelationTemplate(properties[0], source, dest);
		}

		public bool Equals([AllowNull] RelationTemplate other) => !(other is null) && this.Text == other.Text;
	}
}
