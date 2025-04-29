
using Quartiles;

namespace QuartilesTest
{
    [TestClass]
    public class PermutationTests
    {
        [TestMethod]
        public void GetPermutations_PermutationSize1_ReturnsCorrectResult()
        {
            //var chunks = new List<string> { "A", "B", "C", "D", "E", "F" };
            //var expected = new List<string> { "A", "B", "C", "D", "E", "F" };
            //var solver = new QuartilesCracker();

            //solver.GetPermutations([], chunks, 1, false);
            //CollectionAssert.AreEquivalent(expected, solver.results);
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize2_ReturnsCorrectResult()
        {
            var solver = new QuartilesCracker();
            solver.chunks = new List<string> {
                "gest", "lo", "nt", "ut",
                "ger", "di", "ive", "ate",
                "min", "eco", "gi", "ul",
                "stu", "cal", "wo", "man",
                "rum", "or", "mon", "ic",
            };
        }
    }
}