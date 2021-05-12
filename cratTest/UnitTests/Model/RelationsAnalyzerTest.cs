using Xunit;
using System.Collections.Generic;
using CRAT.Model;

namespace CRATTest.UnitTests
{
	public class RelationsAnalyzerTest
    {
        [Fact]
        public void RelationsAnalyzer_LimitValues_NoRelations_True()
        {
            //  Arrange & Act
            var relations = RelationsAnalyzer.AsignLevelsToRelations(new List<Relation>());
            //  Assert
            Assert.Empty(relations);
        }

        [Fact]
        public void RelationsAnalyzer_LevelArrange_SingleEdge_True()
        {
            //  Arrange
            var t = new RelationTemplate("asdf", null, null);
            var relation = new Relation(t, 1, 2);
            List<Relation> relations = new List<Relation>() { relation };

            //  Act
            var res = RelationsAnalyzer.AsignLevelsToRelations(relations);
            var find = res.TryGetValue(relation, out int level);

            //  Assert
            Assert.True(find);
            Assert.Equal(0, level);
        }

        [Theory]
        [MemberData(nameof(RelationAnalyzerData.Data), MemberType = typeof(RelationAnalyzerData))]
        public void RelationsAnalyzer_LevelArrange_MultipleEdges_Theory(List<Relation> relations,
            List<int> results)
        {
            //  Act
            var res = RelationsAnalyzer.AsignLevelsToRelations(relations);
            //  Assert

            for (int i = 0; i < results.Count; i++)
            {
                var find = res.TryGetValue(relations[i], out int level);

                Assert.True(find);
                Assert.Equal(results[i], level);
            }
        }

        public static class RelationAnalyzerData
        {
            public static RelationTemplate t = new RelationTemplate("asdf", null, null);
            public static IEnumerable<object[]> Data =>
                new List<object[]>
                {
                    //  Tests that Relations between two neighbour Annotations are always level 0
                    new object[]
                    {
                        new List<Relation>(){
                            new Relation(t, 1,2),
                            new Relation(t, 6,5),
                            new Relation(t, 4,1),
                        },
                        new List<int>()
                        {
                            0,0,1
                        }
                    },
                    new object[]
                    {
                        new List<Relation>(){
                            new Relation(t, 1,2),
                            new Relation(t, 2,3),
                            new Relation(t, 3,4),
                            new Relation(t, 5,6),
                        },
                        new List<int>()
                        {
                            0,0,0
                        }
                    },
                };
        }
    }
}
