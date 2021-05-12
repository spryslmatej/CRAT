using CRAT.Model;
using System;
using Xunit;

namespace CRATTest.UnitTests.Model
{
	public class AnnotationTest
    {
        [Fact]
        public void Annotation_Constructor_Correct_Success()
        {
            // Arrange & Act & Assert
            var exception = Record.Exception(() => new Annotation(12, "test"));
            Assert.Null(exception);
        }

        [Fact]
        public void Annotation_Constructor_NullString_ThrowsArgumentException()
        {
            //  Act & Assert
            Assert.Throws<ArgumentException>(() => new Annotation(1, null));
        }

        [Fact]
        public void Annotation_Constructor_NegativeIndex_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Annotation(-25, "test"));
        }
    }
}
