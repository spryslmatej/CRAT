using CRAT.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace CRATTest.UnitTests.Model
{
	public class StyleDataTest
	{

		#region Constructor
		[Theory]
		[MemberData(nameof(ConstructorData), MemberType = typeof(StyleDataTest))]
		public void StyleData_Constructor_Correct_Success(ObservableCollection<AnnotationTemplate> at, ObservableCollection<RelationTemplate> rt)
		{
			//  Act & Assert
			var exception = Record.Exception(() => new StyleData(at, rt));
			Assert.Null(exception);
		}

		public static IEnumerable<object[]> ConstructorData =>
			new List<object[]>
			{
                    //  Null data
                    new object[]{
						null,
						null },
                    //  Empty collections
                    new object[]{
						new ObservableCollection<AnnotationTemplate>(),
						new ObservableCollection<RelationTemplate>() },
                    //  One null, other empty collection
                    new object[]{
						null,
						new ObservableCollection<RelationTemplate>() },
                    new object[]{
						new ObservableCollection<AnnotationTemplate>(),
						null },
                    //  Not empty collections
                    new object[]{
						new ObservableCollection<AnnotationTemplate>() { new AnnotationTemplate("asdf", null) },
						new ObservableCollection<RelationTemplate>() { new RelationTemplate("asdf") } },
			};

		[Fact]
		public void StyleData_Constructor_EmptyArguments_Correct_Success()
		{
			//  Act & Assert
			var exception = Record.Exception(() => new StyleData());
			Assert.Null(exception);
		}

		[Theory]
		[InlineData(null)]
		public void SentenceData_Constructor_InvalidArguments_ThrowsArgumentException(string sentence)
		{
			//  Act & Assert
			Assert.Throws<ArgumentException>(() => new SentenceData(sentence)
			);
		}
		#endregion

		#region Templates

		[Theory]
		[MemberData(nameof(StyleDataTestData.AddAnnotationTemplateData),
			MemberType = typeof(StyleDataTestData))]
		public void StyleData_AnnotationTemplates_Add_Theory(
			ObservableCollection<AnnotationTemplate> current,
			AnnotationTemplate toAdd,
			ObservableCollection<AnnotationTemplate> expected)
		{
			//	Arrange
			var styleData = new StyleData(current);
			//  Act & Assert 
			var exception = Record.Exception(() =>
			{
				styleData.AddAnnotationTemplate(toAdd);
				Assert.Equal(styleData.AnnotationTemplates, expected);
			});
			Assert.Null(exception);
		}

		[Theory]
		[MemberData(nameof(StyleDataTestData.RemoveAnnotationTemplateData),
			MemberType = typeof(StyleDataTestData))]
		public void StyleData_AnnotationTemplates_Remove_Theory(
			ObservableCollection<AnnotationTemplate> current,
			AnnotationTemplate toDel,
			bool expectedResult)
		{
			var styleData = new StyleData(current);
			//  Act & Assert 
			var exception = Record.Exception(() =>
			{
				bool res = styleData.RemoveAnnotationTemplate(toDel);
				Assert.Equal(expectedResult, res);
			});
			Assert.Null(exception);
		}

		[Theory]
		[MemberData(nameof(StyleDataTestData.AddRelationTemplateData),
			MemberType = typeof(StyleDataTestData))]
		public void StyleData_RelationTemplates_Add_Theory(
			ObservableCollection<RelationTemplate> current,
			RelationTemplate toAdd,
			ObservableCollection<RelationTemplate> expected)
		{
			//	Arrange
			var styleData = new StyleData(null, current);
			//  Act & Assert 
			var exception = Record.Exception(() =>
			{
				styleData.AddRelationTemplate(toAdd);
				Assert.Equal(styleData.RelationTemplates, expected);
			});
			Assert.Null(exception);
		}

		[Theory]
		[MemberData(nameof(StyleDataTestData.RemoveRelationTemplateData),
				MemberType = typeof(StyleDataTestData))]
		public void StyleData_RelationTemplates_Remove_Theory(
			ObservableCollection<RelationTemplate> current,
			RelationTemplate toDel,
			bool expectedResult)
		{
			var styleData = new StyleData(null, current);
			//  Act & Assert 
			var exception = Record.Exception(() =>
			{
				bool res = styleData.RemoveRelationTemplate(toDel);
				Assert.Equal(expectedResult, res);
			});
			Assert.Null(exception);
		}

		private static class StyleDataTestData
		{
			static readonly AnnotationTemplate annTest = new AnnotationTemplate("test", null);
			static readonly AnnotationTemplate annAdd = new AnnotationTemplate("add", null);

			public static IEnumerable<object[]> AddAnnotationTemplateData 
				=> new List<object[]>
			{
					//	Initialize and add
					new object[]{
						null,
						annAdd,
						new ObservableCollection<AnnotationTemplate>(){ annAdd }
					},
					//	Add
					new object[]{
						new ObservableCollection<AnnotationTemplate>(){ annTest },
						annAdd,
						new ObservableCollection<AnnotationTemplate>(){ annTest, annAdd }
					},
					//	Duplicates check
					new object[]{
						new ObservableCollection<AnnotationTemplate>(){ annTest },
						annTest,
						new ObservableCollection<AnnotationTemplate>(){ annTest }
					}
			};
			public static IEnumerable<object[]> RemoveAnnotationTemplateData => new List<object[]>
			{
					//	Correct
					new object[]{
						new ObservableCollection<AnnotationTemplate>(){ annTest },
						annTest,
						true
					},
					//	Missing
					new object[]{
						new ObservableCollection<AnnotationTemplate>(){ },
						annTest,
						false
					},
					//	Null and missing
					new object[]{
						null,
						annTest,
						false
					},
			};

			static readonly RelationTemplate relTest = new RelationTemplate("test");
			static readonly RelationTemplate relAdd = new RelationTemplate("add");
			public static IEnumerable<object[]> AddRelationTemplateData => new List<object[]>
			{
					//	Initialize and add
					new object[]{
						null,
						relAdd,
						new ObservableCollection<RelationTemplate>(){ relAdd }
					},
					//	Add
					new object[]{
						new ObservableCollection<RelationTemplate>(){ relTest },
						relAdd,
						new ObservableCollection<RelationTemplate>(){ relTest, relAdd }
					},
					//	Duplicates check
					new object[]{
						new ObservableCollection<RelationTemplate>(){ relTest },
						relTest,
						new ObservableCollection<RelationTemplate>(){ relTest }
					}
			};
			public static IEnumerable<object[]> RemoveRelationTemplateData => new List<object[]>
			{
					//	Correct
					new object[]{
						new ObservableCollection<RelationTemplate>(){ relTest },
						relTest,
						true
					},
					//	Missing
					new object[]{
						new ObservableCollection<RelationTemplate>(){ },
						relTest,
						false
					},
					//	Null and missing
					new object[]{
						null,
						relTest,
						false
					},
			};
		}
		#endregion

	}
}
