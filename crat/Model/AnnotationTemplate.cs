using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace CRAT.Model
{
	public sealed class AnnotationTemplate : IEquatable<AnnotationTemplate>
	{
		public string Text { get; }
		public Brush Brush { get; }

		public AnnotationTemplate(string t, Brush b)
		{
			var noWhitespaces = Regex.Replace(t, @"\s+", "");
			if (noWhitespaces.Length == 0)
				throw new ArgumentException("Annotation text cannot be empty.");

			Text = t;
			Brush = b;
		}

		public override string ToString() { return Text + ";" + Brush.ToString(); }
		public static AnnotationTemplate FromString(string s)
		{
			var data = s.Split(";");
			return new AnnotationTemplate(data[0], 
				new BrushConverter().ConvertFromString(data[1]) as SolidColorBrush);
		}

		public bool Equals([AllowNull] AnnotationTemplate other) => !(other is null) && this.Text == other.Text;
	}
}
