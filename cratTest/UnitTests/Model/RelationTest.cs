using Xunit;
using System;
using CRAT.Model;

namespace CRATTest.UnitTests.Model
{
	public class RelationTest
    {
        [Fact]
        public void Relation_Constructor_Correct_Success()
        {
            //  Arrange
            var t = new RelationTemplate("test", null, null);
            //  Act & Assert
            var exception = Record.Exception(() => new Relation(t, 1, 2));
            Assert.Null(exception);
        }


        [Fact]
        public void Relation_Constraints_LeftIndexEqualsRightIndex_ThrowsArgumentException()
        {
            //  Arrange
            var r = new RelationTemplate("test", null, null);
            //  Act & Assert
            Assert.Throws<ArgumentException>(() => new Relation(r, 0, 0));
        }

        [Fact]
        public void Relation_NullTemplate_ThrowsArgumentException()
        {
            //  Act & Assert
            Assert.Throws<ArgumentException>(() => new Relation(null, 0, 1));
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        public void Relation_NegativeIndexes_ThrowsArgumentException(int left, int right)
        {
            //  Arrange
            var r = new RelationTemplate("test", null, null);
            //  Act & Assert
            Assert.Throws<ArgumentException>(() => new Relation(r, left, right));
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(1, 2, 1)]
        [InlineData(5, 6, 1)]
        public void Relation_GetDiff_Works(int res, int left, int right)
        {
            //  Arrange
            var t = new RelationTemplate("test", null, null);
            var r = new Relation(t, left, right);

            //  Act & Assert
            Assert.Equal(res, r.GetDiff());
        }
    }
}
