using System.Diagnostics;

public class QuartilesCracker
{
    // Init dictionary
    // Quartile class. Has information like max size of word chunk, max word chunks, etc.
    // Permutation generator. Given a set of elements, returns a set of all possible permutations of elements.
    // Find word. Given a string, sees if it is a word in the dictionary.

    // Maximum number of word chunks that can be used to form a word.
    private const int MAX_CHUNKS = 2;
    public QuartilesCracker()
    {

    }

    public static void getPermutations(List<string> chunksOutOfList, List<string> chunksInSet, HashSet<string> results)
    {
        if (chunksOutOfList.Count == MAX_CHUNKS)
        {
            // Join the elements into one word and add it to results
            string permutation = string.Join("", chunksOutOfList);
            results.Add(permutation);
            return;
        }

        foreach (var element in chunksInSet)
        {
            var newBag = new List<string>(chunksInSet);
            newBag.Remove(element);

            var newOut = new List<string>(chunksOutOfList) { element };

            getPermutations(newOut, newBag, results);
        }
    }

    public static void PrintPermutationsRecursively()
    {
        var results = new HashSet<string>(); // This will hold all the permutations

        getPermutations(new List<string>(), new List<string> {
            "A", "B", "C", "D", "E", "F",
            "AB", "AC", "AD", "AE",
            "BA", "BE", "BF",
            "ING", "IL", "IT", "IN",
            "IS", "TO", "OF", "ON", "AT", "BO", "PEEE", "JOE"
        },
        results
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