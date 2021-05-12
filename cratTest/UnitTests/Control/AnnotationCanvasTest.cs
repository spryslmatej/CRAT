using CRAT.Control;
using CRAT.Model;
using System.Collections.Generic;
using Xunit;

namespace CRATTest.UnitTests.Control
{
	public class AnnotationCanvasTest
    {
        /*
         * Asserting lack of exception: https://stackoverflow.com/a/65258419
         * 
         * Annotation canvas shouldn't test for anything, therefore it should not throw any exceptions
         */

        [WpfTheory]
        [MemberData(nameof(AnnotationCanvasData.Data), MemberType = typeof(AnnotationCanvasData))]
        public void AnnotationCanvas_DrawSentence_Correct_Success(SentenceData data)
        {
            //  Arrange & Act & Assert
            var exception = Record.Exception(() => new AnnotationCanvas() { SentenceData = data });
            Assert.Null(exception);
        }

        public static class AnnotationCanvasData
        {
            public static IEnumerable<object[]> Data =>
                new List<object[]>
                {
                    //  Null data
                    new object[]
                    {
                        null
                    },
                    
                    //  Empty data
                    new object[]
                    {
                        new SentenceData()
                    },                    
                    new object[]
                    {
                        new SentenceData("")
                    },
                    
                    //  Some data                                        
                    new object[]
                    {
                        new SentenceData("asdf asdf")
                    },
                };
        };
    }
}
