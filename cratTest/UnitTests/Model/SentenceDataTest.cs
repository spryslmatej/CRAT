using CRAT.Model;
using System;
using Xunit;

namespace CRATTest.UnitTests.Model
{
	public class SentenceDataTest
    {
        [Fact]
        public void SentenceData_Constructor_CorrectBehaviour_Success()
        {
            //  Act & Assert
            var exception = Record.Exception(() => new SentenceData("testing"));
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
    }
}
