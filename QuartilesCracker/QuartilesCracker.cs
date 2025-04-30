using System.Diagnostics;

namespace Quartiles;
public class QuartilesCracker
{
    // Maximum number of word chunks that can be used to form a word.
    private const int MAX_CHUNKS = 4;

    // Maximum number of lines (rows) in a quartile game
    private const int MAX_LINES = 5;

    // List of words that solve the quartile
    public List<string> results;

    // Quartiles word chunks (i.e. the words like "gi", "eco", "pp")
    public List<string> chunks;

    // HashSet that contains all words for quick lookup
    public HashSet<string> dictionary;

    // Current dictionary file name
    private string currentDictionary = "oriList";

    // Contains valid word - chunk pairings
    private Dictionary<string, List<string>> wordChunkMapping;
    public QuartilesCracker()
    {
        results = [];
        chunks = [];

        // 2of12.txt
        // scrabble_dictionary.txt
        string path = Path.Combine("Dictionaries", currentDictionary + ".txt");

        dictionary = new HashSet<string>(File.ReadAllLines(path));
        wordChunkMapping = [];
    }

    public void QuartilesDriver()
    {
        List<string> remainingChunks = chunks;
        Console.WriteLine("Stage 1");

        while(results.Count <= MAX_LINES)
        {
            try
            {
                GetPermutations([], remainingChunks, MAX_CHUNKS, true);

                // If reached, means word wasn't found
                // Switch dictionary

            }
            catch(EarlyExitException ex)
            {
                Console.WriteLine($"Found word! {ex.Message}");

                List<string> chunksUsed = wordChunkMapping[ex.Message];
                remainingChunks.RemoveAll(chunks => chunksUsed.Contains(chunks));
            }
        }

        Console.WriteLine("Stage 2");
        for(int chunkSize = MAX_CHUNKS - 1; chunkSize >0; chunkSize--)
        {
            GetPermutations([], chunks, chunkSize, false);
        }
    }

    public void GetPermutations(List<string> chunksOutOfList, List<string> chunksInList, int maxChunks, bool endEarly)
    {
        if (chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if(dictionary.Contains(permutation))
            {
                results.Add(permutation);
                wordChunkMapping.Add(permutation, chunksOutOfList);

                // Triggered when maxChunks equals MAX_CHUNKS (i.e. 4 under normal circumstances)
                if(endEarly)
                {
                    throw new EarlyExitException(permutation);
                }
            }

            return;
        }

        foreach (var chunk in chunksInList)
        {
            var newChunkList = new List<string>(chunksInList);
            newChunkList.Remove(chunk);
            var newChunkOut = new List<string>(chunksOutOfList) { chunk };
            GetPermutations(newChunkOut, newChunkList, maxChunks, endEarly);
        }
    }

    public class EarlyExitException : Exception
    {
        public EarlyExitException(string message) : base(message)
        {
        }
    }

    public static void Main()
    {
        QuartilesCracker solver = new QuartilesCracker();

        solver.chunks = new List<string> {
                "gest", "lo", "nt", "ut",
                "ger", "di", "ive", "ate",
                "min", "eco", "gi", "ul",
                "stu", "cal", "wo", "man",
                "rum", "or", "mon", "ic",
            };

        var stopwatch = Stopwatch.StartNew();  // Start timing

        //PrintPermutationsRecursively();

        Console.WriteLine("Cracking!");

        solver.QuartilesDriver();

        stopwatch.Stop();  // Stop timing
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

    }
}