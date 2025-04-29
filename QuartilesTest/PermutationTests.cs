
using Quartiles;

namespace QuartilesTest
{
    [TestClass]
    public class PermutationTests
    {
        [TestMethod]
        public void GetPermutations_PermutationSize1_ReturnsCorrectPerm()
        {
            var permutations = new HashSet<string>();
            QuartilesCracker.GetPermutations([], ["A", "B", "C"], 1, permutations);
            var expected = new HashSet<string> { "A", "B", "C" };
            CollectionAssert.AreEquivalent(expected.ToList(), permutations.ToList());
        }
    }
}