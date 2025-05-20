namespace Quartiles;
public class QuartilesCracker
{
    // Maximum number of word chunks that can be used to form a word.
    public int MAX_CHUNKS;

    // Maximum number of lines (rows) in a quartile game
    public int MAX_LINES;

    // HashSet that contains all words for quick lookup
    public HashSet<string> dictionary;

    // Current dictionary file name
    private string currentDictionary;

    // Contains valid word - chunk pairings
    public List<KeyValuePair<string, List<string>>> wordChunkMapping;

    private string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));

    /// <summary>
    /// Constructor for QuartilesCracker.
    ///
    /// Defaults to the quartiles dictionary with 4 chunks per word and 5 lines.
    /// </summary>
    public QuartilesCracker(string currentDictionary = "quartiles_dictionary")
    {
        MAX_CHUNKS = 4;
        MAX_LINES = 5;
        
        this.currentDictionary = currentDictionary;
        string dictionaryFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Dictionaries"));
        string dictionaryPath = Path.Combine(dictionaryFolder, currentDictionary + ".txt");

        dictionary = [.. File.ReadAllLines(dictionaryPath)];
        wordChunkMapping = [];
    }

    /// <summary>
    /// Generates all permutations from maximum chunk size to 1 which stores valid words in the results list.
    /// </summary>
    public List<string> QuartilesDriver(List<string> chunks)
    {
        List<string> results = [];

        for(int chunkSize = MAX_CHUNKS; chunkSize > 0; chunkSize--)
        {
            List<string> tempResults = GetPermutations([], chunks, [], chunkSize);

            foreach(string tempResult in tempResults)
            {
                results.Add(tempResult);
            }
        }

        return results;
    }

    /// <summary>
    /// Generates all permutations of the chunks in the list and checks if they are valid words.
    /// Stores valid words in the results list and their chunk mappings in the wordChunkMapping dictionary.
    /// </summary>
    /// <param name="chunksOutOfList">Chunks used in current permutation. Initially empty</param>
    /// <param name="chunksInList">Chunks available to use for permutation. Initially full chunk list</param>
    /// <param name="maxChunks">Maximum amount of chunks to use when permutating</param>
    public List<string> GetPermutations(List<string> chunksOutOfList, List<string> chunksInList, List<string> results, int maxChunks)
    {
        if(chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if(dictionary.Contains(permutation))
            {
                results.Add(permutation);
                wordChunkMapping.Add(new KeyValuePair<string, List<string>>(permutation, chunksOutOfList));
            }

            return results;
        }

        foreach(var chunk in chunksInList)
        {
            var newChunkList = new List<string>(chunksInList);
            newChunkList.Remove(chunk);
            var newChunkOut = new List<string>(chunksOutOfList) { chunk };
            GetPermutations(newChunkOut, newChunkList, results, maxChunks);
        }

        // Ensure a return statement is present for all code paths
        return results;
    }

    /// <summary>
    /// This method verifies that the chunk list is the correct size.
    /// </summary>
    /// <param name="chunks">Chunk list generated from image-text extraction</param>
    /// <exception cref="Exception">Thrown if the size of the list doesn't match board size</exception>
    public void VerifyChunks(List<string> chunks)
    {
        if(chunks.Count != MAX_CHUNKS * MAX_LINES)
        {
            throw new Exception("Chunk list does not match board size!");
        }
    }

    public static void Main()
    {
        //var stopwatch = Stopwatch.StartNew();  // Start timing

        //QuartilesCracker solver = new QuartilesCracker();

        //// Extract image data and store in chunk list
        //var extractor = new QTT("quartiles-unlimited1.png");
        //extractor.ExtractChunks();
        //solver.VerifyChunks(extractor.chunks);
        //solver.chunks = extractor.chunks;

        //// Solve Puzzle
        //Console.WriteLine("Cracking!");
        //solver.QuartilesDriver();

        //stopwatch.Stop();  // Stop timing
        //Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

        //foreach(var word in solver.results)
        //{
        //    Console.WriteLine(word);
        //}
    }
}