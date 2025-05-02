using Quartiles;

namespace QuartilesTest
{
    [TestClass]
    public class PermutationTests
    {
        [TestMethod]
        public void GetPermutations_PermutationSize1_ContainsCorrectResults()
        {
            // 2024-05-30 Quartile
            // https://pbs.twimg.com/media/GPKt8_UasAACWGe.jpg:large
            // https://www.reddit.com/r/quartiles/comments/1d4577k/20240530/
            var solver = new QuartilesCracker();
            solver.chunks = new List<string> {
                "gest", "lo", "nt", "ut",
                "ger", "di", "ive", "ate",
                "min", "eco", "gi", "ul",
                "stu", "cal", "wo", "man",
                "rum", "or", "mon", "ic",
            };

            var expected = new List<string> { "ate", "gi", "man", "rum", "or" };

            solver.GetPermutations([], solver.chunks, 1);

            foreach(string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize2_ContainsCorrectResults()
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
                "digest", "dint", "gestate", "local", "lout", "manger", 
                "manic", "manor", "minor", "orate", "rumor",
                "stunt", "woman", "wont"
            };

            solver.GetPermutations([], solver.chunks, 2);

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize3_ContainsCorrectResults()
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
                "callout", "caloric", "digestive", "germinate", "logical", 
                "stuntman", 
            };

            solver.GetPermutations([], solver.chunks, 3);

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize4_ContainsCorrectResults()
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
                "diminutive", "ecological", "gesticulate", "rumormonger", "stuntwoman"
            };

            solver.GetPermutations([], solver.chunks, 4);

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }
    }
}