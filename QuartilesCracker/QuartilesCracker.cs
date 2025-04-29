using Microsoft.VisualBasic;
using System.Diagnostics;

namespace Quartiles;
public class QuartilesCracker
{
    // Init dictionary
    // Quartile class. Has information like max size of word chunk, max word chunks, etc.
    // Permutation generator. Given a set of elements, returns a set of all possible permutations of elements.
    // Find word. Given a string, sees if it is a word in the dictionary.

    // Maximum number of word chunks that can be used to form a word.
    private const int MAX_CHUNKS = 5;
    public List<string> results;
    public List<string> chunks;
    public QuartilesCracker()
    {
        results = [];
        chunks = [];
    }

    public void QuartilesDriver()
    {

    }

    public void GetPermutations(List<string> chunksOutOfList, List<string> chunksInSet, int maxChunks, bool endEarly)
    {
        if (chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if(IsWord(permutation))
            {
                results.Add(permutation);

                // Triggered when maxChunks equals MAX_CHUNKS (i.e. 5 under normal circumstances)
                if(endEarly)
                {
                    throw new EarlyExitException();
                }
            }

            return;
        }

        foreach (var element in chunksInSet)
        {
            var newBag = new List<string>(chunksInSet);
            newBag.Remove(element);
            var newOut = new List<string>(chunksOutOfList) { element };
            GetPermutations(newOut, newBag, maxChunks, endEarly);
        }
    }

    //public static void PrintPermutationsRecursively()
    //{
    //    var permutations = new HashSet<string>();

    //    GetPermutations([], [
    //        "A", "B", "C", "D", "E", "F",
    //        "AB", "AC", "AD", "AE",
    //        "BA", "BE", "BF",
    //        "ING", "IL", "IT", "IN",
    //        "IS", "TO", "OF", "ON", "AT", "BO", "PEEE", "JOE"
    //    ],
    //    MAX_CHUNKS,
    //    true
    //    );

    //    // Print all the results
    //    //foreach (var word in results)
    //    //{
    //    //    Console.WriteLine(word);
    //    //}

    //    //Console.WriteLine($"Total permutations: {results.Count}");
    //}

    public static bool IsWord(string word)
    {
        // If word is in dictionary, return true
        return true;
    }

    public class EarlyExitException : Exception { }

    public static void Main()
    {
        var stopwatch = Stopwatch.StartNew();  // Start timing

        //PrintPermutationsRecursively();

        stopwatch.Stop();  // Stop timing
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

    }
}