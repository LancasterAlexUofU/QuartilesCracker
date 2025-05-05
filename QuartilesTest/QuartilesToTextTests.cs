using QuartilesToText;

namespace QuartilesTest
{
    [TestClass]
    public class QuartilesToTextTests
    {
        [TestMethod]
        public void ExtractChunks_ExtractQuartiles1_ReturnsCorrectList()
        {
            var extractor = new QTT("quartiles1.png");
            extractor.ExtractChunks();
            
            List<string> expected = new List<string> {
                "va", "ent", "ga", "ti",
                "pon", "ex", "wan", "cul",
                "dis", "en", "re", "rd",
                "lu", "ser", "st", "ity",
                "dip", "der", "tes", "ial"
            };

            CollectionAssert.AreEquivalent(expected, extractor.chunks);
        }

        [TestMethod]
        public void ExtractChunks_ExtractQuartiles2_ReturnsCorrectList()
        {
            var extractor = new QTT("quartiles2.png");
            extractor.ExtractChunks();

            List<string> expected = new List<string> {
                "dis", "ent", "ga", "rd",
                "pon", "ex", "re", "ial",
                "wan", "der", "lu", "st",
                "ser", "en", "dip", "ity",
                "cul", "ti", "va", "tes"
            };

            CollectionAssert.AreEquivalent(expected, extractor.chunks);
        }
    }
}