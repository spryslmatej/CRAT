using System;
using System.Text.RegularExpressions;

namespace CRAT.Model
{
	public class Annotation
	{
		public int TokenIndex { get; }
		public string Text { get; }

		public Annotation(int index, string text)
		{
			if (text is null)
				throw new ArgumentException("Text cannot be null.");
			var noWhitespaces = Regex.Replace(text, @"\s+", "");
			if (noWhitespaces.Length == 0)
				throw new ArgumentException("Annotation text cannot be empty.");

			if (index < 0)
				throw new ArgumentException("Index cannot be <0.");

			Text = text;
			TokenIndex = index;
		}

		public override string ToString() { return TokenIndex + " " + Text; }
		public static Annotation FromString(string s)
		{
			try
			{
				var data = s.Split(" ");
				return new Annotation(Convert.ToInt32(data[0]), data[1]);
			}
			catch
			{ return null; }
		}
	}
}
