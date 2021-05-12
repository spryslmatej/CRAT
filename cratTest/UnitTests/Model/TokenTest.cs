using CRAT.Model;
using System;
using Xunit;

namespace CRATTest.UnitTests.Model
{
	public class TokenTest
    {
        [Fact]
        public void Relation_Constructor_Correct_Success()
        {
            //  Act & Assert
            var exception = Record.Exception(() => new Token("testing"));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        public void Token_EmptyText_ThrowsArgumentException(string s)
        {
            //  Act & Assert
            Assert.Throws<ArgumentException>(() => new Token(s));
        }
    }
}
