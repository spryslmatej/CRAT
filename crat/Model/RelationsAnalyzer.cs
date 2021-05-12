using System.Collections.Generic;

namespace CRAT.Model
{
	public static class RelationsAnalyzer
	{
		public static Dictionary<Relation, int> AsignLevelsToRelations(List<Relation> myEdges)
		{
			Dictionary<Relation, int> output = new Dictionary<Relation, int>();

			if (myEdges.Count == 0)
				return output;

			myEdges.Sort(Relation.CompareByDiff);

			//  Get max right index
			int maxIndex = int.MinValue;
			myEdges.ForEach(item =>
			{
				if (item.RightIndex > maxIndex) maxIndex = item.RightIndex;
			});

			int dpSize = maxIndex + maxIndex - 1;  // MaxIndex = count of Tokens, MaxIndex-1 = count of spaces between
			dpSize++;

			//  Initialize DP array
			int[] dp = new int[dpSize];
			for (int i = 0; i < dpSize; i++)
				dp[i] = i % 2 == 0 ? 1 : 0; //  Every even index contains a possible annotation which we don't want to interrupt

			//    |--------------|
			//    |-----|  |-----|
			//  t1 space t2 space t3 space ...
			//  0    1   2    3   4    5   ...

			foreach (var item in myEdges)
			{
				int level = 0;

				//  Find highest level between Left and Right indexes
				var leftmostSpace = item.LeftIndex * 2 + 1;
				var rightmostSpace = item.RightIndex * 2 - 1;
				for (int i = leftmostSpace; i <= rightmostSpace; i++)
					if (dp[i] > level)
						level = dp[i];

				//	Add item to output
				output.Add(item, level);

				//  Increment all values between current indexes
				for (int i = leftmostSpace; i <= rightmostSpace; i++)
					dp[i] = level + 1;
			}

			return output;
		}
	}
}
