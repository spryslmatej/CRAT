using CRAT.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace CRATTest.UnitTests.Model
{
	public class RelationTemplateTest
	{
		[Fact]
		public void RelationTemplate_Constructor_NullLists_Correct_Success()
		{
			//  Act & Assert
			var exception = Record.Exception(() => new RelationTemplate("testing", null, null));
			Assert.Null(exception);
		}

		[Fact]
		public void RelationTemplate_Constructor_Lists_Correct_Success()
		{
			//  Act & Assert
			var exception = Record.Exception(() => new RelationTemplate("testing", new List<string>(), new List<string>()));
			Assert.Null(exception);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\t")]
		public void RelationTemplate_EmptyText_ThrowsArgumentException(string s)
		{
			//  Act & Assert
			Assert.Throws<ArgumentException>(() => new RelationTemplate(s, null, null));
		}

		[Theory]
		[InlineData("a", "b")]
		public void RelationTemplate_AnnotationsTemplates_AddAnnotations_Correct_True(string src, string dest)
		{
			//	Arrange
			var template = new RelationTemplate("test", null, null);

			//  Act & Assert
			Assert.True(template.AddSourceAnnotation(src));
			Assert.True(template.AddDestinationAnnotation(dest));
		}

		[Theory]
		[InlineData("a", "b")]
		public void RelationTemplate_AnnotationsTemplates_AddAnnotations_Duplicates_False(string src, string dest)
		{
			//	Arrange
			var sourceAnno = new List<string> { "a", "b" };
			var destAnno = new List<string> { "a", "b" };
			var template = new RelationTemplate("test", sourceAnno, destAnno);

			//  Act & Assert
			Assert.False(template.AddSourceAnnotation(src));
			Assert.False(template.AddDestinationAnnotation(dest));
		}

		[Theory]
		[InlineData("", "")]
		[InlineData(null, null)]
		public void RelationTemplate_AnnotationsTemplates_AddAnnotations_EmptyOrNull_False(string src, string dest)
		{
			//	Arrange
			var template = new RelationTemplate("test", null, null);

			//  Act & Assert
			Assert.False(template.AddSourceAnnotation(src));
			Assert.False(template.AddDestinationAnnotation(dest));
		}

		[Theory]
		[InlineData("a", "a")]
		public void RelationTemplate_AnnotationsTemplates_RemoveAnnotations_Correct_True(string src, string dest)
		{
			//	Arrange
			var sourceAnno = new List<string> { "a" };
			var destAnno = new List<string> { "a" };
			var template = new RelationTemplate("test", sourceAnno, destAnno);

			//  Act & Assert
			Assert.True(template.RemoveSourceAnnotation(src));
			Assert.True(template.RemoveDestinationAnnotation(dest));
		}

		[Theory]
		[InlineData("b", "b")]
		public void RelationTemplate_AnnotationsTemplates_RemoveAnnotations_NotFound_False(string src, string dest)
		{
			//	Arrange
			var sourceAnno = new List<string> { "a" };
			var destAnno = new List<string> { "a" };
			var template = new RelationTemplate("test", sourceAnno, destAnno);

			//  Act & Assert
			Assert.False(template.RemoveSourceAnnotation(src));
			Assert.False(template.RemoveDestinationAnnotation(dest));
		}
	}
}
