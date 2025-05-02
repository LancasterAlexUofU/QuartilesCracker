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
    private string currentDictionary = "quartiles_dictionary";

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
        for(int chunkSize = MAX_CHUNKS; chunkSize > 0; chunkSize--)
        {
            GetPermutations([], chunks, chunkSize);
        }
    }

    public void GetPermutations(List<string> chunksOutOfList, List<string> chunksInList, int maxChunks)
    {
        if (chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if(dictionary.Contains(permutation))
            {
                results.Add(permutation);
                wordChunkMapping.Add(permutation, chunksOutOfList);
            }

            return;
        }

        foreach (var chunk in chunksInList)
        {
            var newChunkList = new List<string>(chunksInList);
            newChunkList.Remove(chunk);
            var newChunkOut = new List<string>(chunksOutOfList) { chunk };
            GetPermutations(newChunkOut, newChunkList, maxChunks);
        }
    }

    public static void Main()
    {
        var stopwatch = Stopwatch.StartNew();  // Start timing

        QuartilesCracker solver = new QuartilesCracker();

        solver.chunks = new List<string> {
            "og", "hic", "od", "ara",
            "sc", "ella", "nks", "wi",
            "rap", "dem", "ly", "ny",
            "ent", "ial", "cam", "ho",
            "mi", "rie", "pot", "de",
        };

        Console.WriteLine("Cracking!");

        solver.QuartilesDriver();

        stopwatch.Stop();  // Stop timing
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

        foreach (var word in solver.results)
        {
            Console.WriteLine(word);
        }
    }
}