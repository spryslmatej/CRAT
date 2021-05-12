using CRAT.Model;
using System.Collections.Generic;
using Xunit;

namespace CRATTest.UnitTests
{
	public class ModelValidatorTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(6)]
        public void ModelValidator_ValidateSentenceData_AnnotationOutOfBounds_Fails(int index)
        {
            //  Arrange
            List<Token> t = new List<Token>();

            List<Annotation> a = new List<Annotation>() { new Annotation(index, "asdf") };

            List<Relation> r = new List<Relation>();

            var s = new SentenceData(t, a, r);

            //  Act & Assert
            Assert.False(ModelValidator.ValidateSentenceData(s));
        }

        [Theory]
        [InlineData(10, 11)]
        [InlineData(0, 5)]
        [InlineData(5, 0)]
        public void ModelValidator_ValidateSentenceData_RelationOutOfBounds_Fails(int left, int right)
        {
            //  Arrange
            List<Token> t = new List<Token>();

            List<Annotation> a = new List<Annotation>();

            List<Relation> r = new List<Relation>() {
                new Relation(new RelationTemplate("test", null, null), left, right)
            };

            var s = new SentenceData(t, a, r);

            //  Act & Assert
            Assert.False(ModelValidator.ValidateSentenceData(s));
        }

        [Theory]
        [MemberData(nameof(ValidatorData.Data), MemberType = typeof(ValidatorData))]
        public void ModelValidator_ValidateSentenceData_Correct_Success(List<Token> t, List<Annotation> a, List<Relation> r)
        {
            //  Arrange
            var s = new SentenceData(t, a, r);

            //  Act & Assert
            Assert.True(ModelValidator.ValidateSentenceData(s));
        }
        //  idea source:  https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/

        public static class ValidatorData
        {
            public static IEnumerable<object[]> Data =>
                new List<object[]>
                {

                new object[]
                {
                    new List<Token>(), new List<Annotation>(), new List<Relation>(),
                },

                new object[]
                {
                    new List<Token>()
                    {
                        new Token("asdf"),
                        new Token("asdf")
                    },
                    new List<Annotation>()
                    {
                        new Annotation(0,"asdf"),
                        new Annotation(1,"asdf")
                    },
                    new List<Relation>()
                    {
                        new Relation(new RelationTemplate("test", null, null), 0, 1)
                    },
                },

                new object[]
                {
                    new List<Token>()
                    {
                        new Token("asdf"),
                        new Token("asdf"),
                    },
                    new List<Annotation>()
                    {
                        new Annotation(1,"asdf")
                    },
                    new List<Relation>()
                    {
                    },
                },

                };
        };
    }
}
