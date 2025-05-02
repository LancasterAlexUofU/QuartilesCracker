using Quartiles;
namespace QuartilesTest
{
    [TestClass]
    public class QuartilesTests
    {
        [TestMethod]
        public void QuartilesDriver_SolveQuartile1_ReturnsCorrectResult()
        {
            var solver = new QuartilesCracker();
            solver.chunks = new List<string> {
                "gest", "lo", "nt", "ut",
                "ger", "di", "ive", "ate",
                "min", "eco", "gi", "ul",
                "stu", "cal", "wo", "man",
                "rum", "or", "mon", "ic",
            };

            var expected = new List<string> {
                "diminutive", "ecological", "gesticulate", "rumormonger", "stuntwoman",
                "callout", "caloric", "digestive", "germinate", "logical",
                "stuntman", "digest", "dint", "gestate", "local", 
                "lout", "manger", "manic", "manor", "minor", 
                "orate", "rumor", "stunt", "woman", "wont",
                "ate", "gi", "man", "rum", "or"
            };

            solver.QuartilesDriver();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }
    }
}