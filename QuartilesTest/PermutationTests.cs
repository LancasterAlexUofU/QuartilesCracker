using Quartiles;
using Paths;

namespace QuartilesTest
{

    [TestClass]
    public class PermutationTests
    {

        private QuartilesCracker solver;
        private List<string> chunks;

        [TestInitialize]
        public void Setup()
        {
            solver = new QuartilesCracker();
            solver.CurrentDictionary = "quartiles_dictionary";

            //// 2024-05-30 Quartile
            //// https://pbs.twimg.com/media/GPKt8_UasAACWGe.jpg:large
            //// https://www.reddit.com/r/quartiles/comments/1d4577k/20240530/
            chunks = new List<string> {
            "gest", "lo", "nt", "ut",
            "ger", "di", "ive", "ate",
            "min", "eco", "gi", "ul",
            "stu", "cal", "wo", "man",
            "rum", "or", "mon", "ic",
            };
        }

        [TestMethod]
        public void GetPermutations_PermutationSize1_ContainsCorrectResults()
        {
            var expected = new HashSet<string> { "ate", "gi", "man", "rum", "or" };
            HashSet<string> solutions = [];
            solver.GetPermutations([], chunks, solutions, 1);

            var solList = solutions.ToList();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solList, word, "Solutions does not contain all of expected");
            }
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize2_ContainsCorrectResults()
        {
            var expected = new List<string> { 
                "digest", "dint", "gestate", "local", "lout", "manger", 
                "manic", "manor", "minor", "orate", "rumor",
                "stunt", "woman", "wont"
            };

            HashSet<string> solutions = [];
            solver.GetPermutations([], chunks, solutions, 2);

            var solList = solutions.ToList();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solList, word, "Solutions does not contain all of expected");
            }
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize3_ContainsCorrectResults()
        {
            var expected = new List<string> { 
                "callout", "caloric", "digestive", "germinate", "logical", 
                "stuntman", 
            };

            HashSet<string> solutions = [];
            solver.GetPermutations([], chunks, solutions, 3);

            var solList = solutions.ToList();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solList, word, "Solutions does not contain all of expected");
            }
        }

        [TestMethod]
        public void GetPermutations_PermutationsSize4_ContainsCorrectResults()
        {
            var expected = new List<string> {
                "diminutive", "ecological", "gesticulate", "rumormonger", "stuntwoman"
            };

            HashSet<string> solutions = [];
            solver.GetPermutations([], chunks, solutions, 4);

            var solList = solutions.ToList();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solList, word, "Solutions does not contain all of expected");
            }
        }
    }
}