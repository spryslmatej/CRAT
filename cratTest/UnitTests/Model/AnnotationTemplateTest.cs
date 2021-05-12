using CRAT.Model;
using System;
using Xunit;

namespace CRATTest.UnitTests.Model
{
	public class AnnotationTemplateTest
    {

        [Fact]
        public void AnnotationTemplate_Constructor_Correct_Success()
        {
            //  Act & Assert
            var exception = Record.Exception(() => new AnnotationTemplate("testing", null));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        public void AnnotationTemplate_Constructor_EmptyText_ThrowsArgumentException(string s)
        {
            //  Act & Assert
            Assert.Throws<ArgumentException>(() => new AnnotationTemplate(s, null));
        }
    }
}
