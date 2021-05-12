using System;
using System.Collections.Generic;

namespace CRAT.Model
{
	public class ModelData
	{
		public ModelData() : this(null, null) { }
		public ModelData(SentenceData sentence, StyleData style)
		{
			if (sentence is null)
				sentence = new SentenceData();

			if (style is null)
				style = new StyleData();


			SentenceData = sentence;
			StyleData = style;
		}

		public SentenceData SentenceData { get; }
		public StyleData StyleData { get; }


		#region Random Data Generation
		/*
		 * This code will be deleted in the final version
		 */
		public void GenerateRandomData()
		{
			GenerateRandomIndexes();
			if (StyleData.AnnotationTemplates.Count != 0)
			{
				GetRandomAnnotations();
				if (StyleData.RelationTemplates.Count != 0)
					GetRandomRelations();
			}
		}
		private List<int> randomNumbers = new List<int>();
		private void GenerateRandomIndexes()
		{
			randomNumbers = new List<int>();
			Random rnd = new Random();

			int uniqueNumbersCount = 7;
			if (SentenceData.Tokens.Count < uniqueNumbersCount)
				uniqueNumbersCount = SentenceData.Tokens.Count;
			for (int i = 0; i < uniqueNumbersCount; i++)
			{
				int num = rnd.Next(0, SentenceData.Tokens.Count);
				if (!randomNumbers.Contains(num))
					randomNumbers.Add(num);
				else
					i--;
			}
		}
		private void GetRandomAnnotations()
		{
			Random rnd = new Random();

			foreach (var item in randomNumbers)
			{
				AnnotationTemplate template = StyleData.AnnotationTemplates[rnd.Next(0, StyleData.AnnotationTemplates.Count)];

				SentenceData.AddAnnotation(template, item);
			}
		}
		private void GetRandomRelations()
		{
			Random rnd = new Random();

			if (randomNumbers.Count < 2)
				return;

			int uniquePairs = 5;
			for (int i = 0; i < uniquePairs; i++)
			{
				int l = rnd.Next(0, randomNumbers.Count);
				int r = rnd.Next(0, randomNumbers.Count);
				if (l == r)
				{
					i--;
					continue;
				}

				l = randomNumbers[l];
				r = randomNumbers[r];

				RelationTemplate template = StyleData.RelationTemplates[rnd.Next(0, StyleData.RelationTemplates.Count)];
				SentenceData.AddRelation(template, l, r);
			}
		}
		#endregion
	}
}
