using QuartilesToText;
using System.Text;

namespace QuartilesTest
{
    [TestClass]
    public class QuartilesToTextTests
    {
        private QuartilesOCR extractor = new QuartilesOCR();

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_06_16_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-06-16.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "ess", "br", "epi", "ol",
                "semi", "la", "ar", "pi",
                "me", "ity", "yer", "lin",
                "ogy", "ick", "cal", "sol",
                "ti", "tro", "demi", "id"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_06_18_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-06-18.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "tor", "ts", "velo", "pas", "ls",
                "too", "tho", "om", "ri", "foo",
                "ci", "ful", "rap", "enc", "ses",
                "rcu", "al", "ug", "me", "ht"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_06_19_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-06-19.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "ju", "li", "un", "vol", "ted",
                "io", "con", "nth", "eau", "st",
                "tma", "ga", "nt", "ll", "por",
                "ream", "ne", "tri", "red", "tee"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_06_20_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-06-20.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "sun", "sol", "tch", "es", "ree",
                "sc", "st", "wi", "he", "cra",
                "ns", "ft", "ic", "mi", "sph",
                "ere", "lum", "in", "os", "ity"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_06_21_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-06-21.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "adj", "gr", "ic", "pl", "ne",
                "ted", "apo", "sto", "ot", "to",
                "ine", "mal", "mot", "au", "us",
                "ll", "ect", "ind", "gui", "ive"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_06_22_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-06-22.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "pri", "no", "cha", "ho", "chr",
                "li", "ro", "ates", "tic", "app",
                "ome", "se", "on", "ma", "ris",
                "ng", "riz", "coun", "mo", "tal"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2024_11_06_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2024-11-06.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "rs", "ena", "pe", "li",
                "hea", "tor", "ri", "br",
                "rt", "scra", "jus", "sky",
                "ia", "ne", "bly", "ter",
                "tif", "ial", "oken", "adr"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2025_03_20_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2025-03-20.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "sa", "ion", "pher", "gra", "non",
                "ct", "ws", "re", "cro", "is",
                "fi", "ico", "on", "ing", "com",
                "lex", "par", "ag", "bot", "sca"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2025_03_21_ReturnsCorrectList()
        {
            extractor.ImageName = "quartiles-2025-03-21.png";
            var chunks = extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "mark", "irv", "ats", "eran", "unre",
                "mo", "cla", "ce", "oya", "ab",
                "pe", "sca", "le", "sev", "har",
                "nt", "dis", "ny", "go", "per"
            };

            CollectionAssert.AreEquivalent(expected, chunks, GetDifferenceMessage(expected, chunks));
        }



        private string GetDifferenceMessage(List<string> expected, List<string> actual)
        {
            var missing = expected.Except(actual).ToList();
            var unexpected = actual.Except(expected).ToList();

            var message = new StringBuilder();

            if (missing.Any())
                message.AppendLine("Missing from actual: " + string.Join(", ", missing));

            if (unexpected.Any())
                message.AppendLine("Unexpected in actual: " + string.Join(", ", unexpected));

            if (!missing.Any() && !unexpected.Any())
                message.AppendLine("Lists are equivalent.");

            return message.ToString();
        }
    }
}