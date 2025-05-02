using Quartiles;
namespace QuartilesTest
{
    [TestClass]
    public class QuartilesTests
    {
        [TestMethod]
        public void QuartilesDriver_SolveQuartile1_ReturnsCorrectResult()
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

        [TestMethod]
        public void QuartilesDriver_SolveQuartile2_ReturnsCorrectResult()
        {
            // 2024-09-17 Quartile
            // https://www.reddit.com/r/quartiles/comments/1fiytvj/todays_theme/
            // https://www.reddit.com/r/quartiles/comments/1fiyxpf/20240917/
            var solver = new QuartilesCracker();
            solver.chunks = new List<string> {
                "og", "hic", "od", "ara",
                "sc", "ella", "nks", "wi",
                "rap", "dem", "ly", "ny",
                "ent", "ial", "cam", "ho",
                "mi", "rie", "pot", "de",
            };

            var expected = new List<string> {
                "camaraderie", "demographic", "hoodwinks", "miscellany", "potentially",
                "cam", "deny", "depot", "descent", "entrap",
                "holy", "homily", "hominy", "honks", "hood",
                "minks", "pot", "potent", "potential", "potently",
                "rap", "scent", "scrap", "wide", "widely",
                "wily", "winks", "winy"
            };

            solver.QuartilesDriver();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }

        [TestMethod]
        public void QuartilesDriver_SolveQuartile3_ReturnsCorrectResult()
        {
            // 2024-11-10 Quartile
            // https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTMhYJwguUaqFgi3ytlfKjZ6RLjlYrh2OcivA&s
            var solver = new QuartilesCracker();
            solver.chunks = new List<string> {
                "ter", "ch", "fl", "wo",
                "age", "od", "ta", "ate",
                "quis", "acc", "con", "gro",
                "at", "dor", "box", "ou",
                "omm", "cam", "rk", "und"
            };

            var expected = new List<string> {
                "accommodate", "camouflage", "chatterbox", "conquistador", "groundwork",
                "age", "at", "ate", "attach", "box",
                "boxwood", "cam", "chat", "chatter", "con",
                "conch", "condor", "conflate", "flat", "flatter",
                "groat", "ground", "ouch", "outer", "tater",
                "wood", "work", "wound"
            };

            solver.QuartilesDriver();

            foreach (string word in expected)
            {
                CollectionAssert.Contains(solver.results, word);
            }
        }
    }
}