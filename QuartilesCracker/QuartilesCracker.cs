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
    public QuartilesCracker()
    {

    }

    public static void GetPermutations(List<string> chunksOutOfList, List<string> chunksInSet, int maxChunks, HashSet<string> permutations)
    {
        if (chunksOutOfList.Count == maxChunks)
        {
            // Join the elements into one word and add it to results
            string permutation = string.Join("", chunksOutOfList);
            permutations.Add(permutation);
            return;
        }

        foreach (var element in chunksInSet)
        {
            var newBag = new List<string>(chunksInSet);
            newBag.Remove(element);

            var newOut = new List<string>(chunksOutOfList) { element };

            GetPermutations(newOut, newBag, maxChunks, permutations);
        }
    }

    public static void PrintPermutationsRecursively()
    {
        var permutations = new HashSet<string>();

        GetPermutations([], [
            "A", "B", "C", "D", "E", "F",
            "AB", "AC", "AD", "AE",
            "BA", "BE", "BF",
            "ING", "IL", "IT", "IN",
            "IS", "TO", "OF", "ON", "AT", "BO", "PEEE", "JOE"
        ],
        MAX_CHUNKS,
        permutations
        );

        // Print all the results
        //foreach (var word in results)
        //{
        //    Console.WriteLine(word);
        //}

        //Console.WriteLine($"Total permutations: {results.Count}");
    }



    public static void Main()
    {
        var stopwatch = Stopwatch.StartNew();  // Start timing

        PrintPermutationsRecursively();

        stopwatch.Stop();  // Stop timing
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

    }
}