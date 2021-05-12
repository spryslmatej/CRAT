using System;

namespace CRAT.Model
{
	public class Relation
	{
		public int LeftIndex { get; }
		public int RightIndex { get; }

		public bool LeftToRightFlow { get; }
		public string Text { get; }

		public Relation(RelationTemplate rt, int left, int right)
		{
			if (left == right || left < 0 || right < 0)
				throw new ArgumentException("Relation indexes cannot be the same, and have to be valid.");
			if (rt is null)
				throw new ArgumentException("Relation has to be built from a template.");

			LeftIndex = Math.Min(left, right);
			RightIndex = Math.Max(left, right);

			LeftToRightFlow = left < right;

			Text = rt.Text;
		}

		public int GetSourceIndex() { return LeftToRightFlow ? LeftIndex : RightIndex; }
		public int GetDestinationIndex() { return LeftToRightFlow ? RightIndex : LeftIndex; }

		public int GetDiff() { return Math.Abs(LeftIndex - RightIndex); }
		public static int CompareByDiff(Relation l, Relation r) { return l.GetDiff().CompareTo(r.GetDiff()); }

		public override string ToString()
		{
			return LeftToRightFlow ?
				LeftIndex + " " + RightIndex + " " + Text :
				RightIndex + " " + LeftIndex + " " + Text;
		}
		public static Relation FromString(string s)
		{
			try
			{
				var data = s.Split(" ");
				var template = new RelationTemplate(data[2], null, null);
				return new Relation(template, Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
			}
			catch
			{ return null; }
		}
	}
}
