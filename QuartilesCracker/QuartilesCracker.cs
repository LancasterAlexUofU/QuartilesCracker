namespace Quartiles;

/// <summary>
/// Class that solves a quartiles game given a list of the letter tiles
/// </summary>
public class QuartilesCracker
{
    // Gets and sets the maximum number of words chunks that can be used to form a word
    public int MaxChunks { get; set; } = 4;

    // Gets and sets the maximum number of lines (rows) in a quartile game
    public int MaxLines { get; set; } = 5;

    private string _currentDictionary = "quartiles_dictionary";

    // Gets and Sets the filename (without file extension) for the dictionary to search for words and automatically reloads the dictionary if it is changed by the user
    public string CurrentDictionary
    {
        get => _currentDictionary;
        set
        {
            if (_currentDictionary != value)
            {
                _currentDictionary = value;
                LoadDictionary();  // Automatically reload when name changes
            }
        }
    }

    // Path to where the Dictionary Folder exists (inside debug folder)
    private static readonly string dictionaryFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Dictionaries"));

    // Path to the Dictionary file
    private string DictionaryPath => Path.Combine(dictionaryFolder, $"{CurrentDictionary}.txt");

    // HashSet that contains all words for quick lookup
    private HashSet<string> dictionary = [];

    /// <summary>
    /// Constructor for QuartilesCracker that initializes the current dictionary
    /// </summary>
    public QuartilesCracker()
    {
        LoadDictionary();
    }

    /// <summary>
    /// Finds all Quartile solutions from max sized chunk solutions through 1 size chunk solutions
    /// </summary>
    /// <param name="chunks">The letters found in a Quartiles game of size rows times columns</param>
    /// <returns>A tuple, with the first element containing all solutions and the second element containing the mapping for which chunks make up a solution</returns>
    public (List<string> allSolutions, List<KeyValuePair<string, List<string>>> solutionChunkMapping) QuartileSolver(List<string> chunks)
    {
        VerifyChunks(chunks);

        List<string> allSolutions = [];
        List<KeyValuePair<string, List<string>>> solutionChunkMapping = [];

        for (int chunkSize = MaxChunks; chunkSize > 0; chunkSize--)
        {
            GetPermutations([], chunks, allSolutions, solutionChunkMapping, chunkSize);
        }

        RemoveDuplicates(allSolutions, solutionChunkMapping);
        return (allSolutions, solutionChunkMapping);
    }

    /// <summary>
    /// Generates all permutations of the chunks in the list and checks if they are valid words
    /// </summary>
    /// <param name="chunksOutOfList">Chunks used in current permutation. Initially empty</param>
    /// <param name="chunksInList">Chunks available to use for permutation. Initially full chunk list</param>
    /// <param name="solutions">List where solutions are stored within recursive calls. Initially empty</param>
    /// <param name="solutionChunkMapping">List where solution-chunk mappings are stored within recursive calls. Initially empty/param>
    /// <param name="maxChunks">Maximum amount of chunks to use when permutating</param>
    protected void GetPermutations(List<string> chunksOutOfList, List<string> chunksInList, List<string> solutions, List<KeyValuePair<string, List<string>>> solutionChunkMapping, int maxChunks)
    {
        if(chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if(dictionary.Contains(permutation))
            {
                solutions.Add(permutation);
                solutionChunkMapping.Add(new KeyValuePair<string, List<string>>(permutation, [.. chunksOutOfList]));
            }

            return;
        }

        foreach(var chunk in chunksInList)
        {
            List<string> newChunkList = [.. chunksInList];
            newChunkList.Remove(chunk);
            List<string> newChunkOut = new(chunksOutOfList) { chunk };
            GetPermutations(newChunkOut, newChunkList, solutions, solutionChunkMapping, maxChunks);
        }
    }

    /// <summary>
    /// This method verifies that the chunk list is the correct size.
    /// </summary>
    /// <param name="chunks">The letters found in a Quartiles game</param>
    /// <exception cref="Exception">Thrown if the size of the list doesn't match board size</exception>
    private void VerifyChunks(List<string> chunks)
    {
        if(chunks.Count != MaxChunks * MaxLines)
        {
            throw new Exception("Chunk list does not match board size!");
        }
    }

    private void VerifyDictionary()
    {
        if(!File.Exists(DictionaryPath))
        {
            throw new FileNotFoundException($"Dictionary file not found: {DictionaryPath}");
        }
    }

    /// <summary>
    /// Removes duplicate solutions and saves it to the ORIGINAL passed parameter
    /// </summary>
    /// <param name="solutions">Solution list to remove duplicates from</param>
    /// <param name="solutionChunkMapping">Solution mapping list to remove duplicates from</param>
    private void RemoveDuplicates(List<string> solutions, List<KeyValuePair<string, List<string>>> solutionChunkMapping)
    {
        var uniqueSolutions = solutions.Distinct().ToList();

        var uniqueMappings = solutionChunkMapping
                                .GroupBy(kvp => kvp.Key)
                                .Select(g => g.First())
                                .ToList();

        solutions.Clear();
        solutions.AddRange(uniqueSolutions);

        solutionChunkMapping.Clear();
        solutionChunkMapping.AddRange(uniqueMappings);
    }

    private void LoadDictionary()
    {
        VerifyDictionary();
        dictionary = [.. File.ReadAllLines(DictionaryPath)];
    }
}