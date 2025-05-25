using Paths;

namespace Quartiles;

/// <summary>
/// Class that solves a quartiles game given a list of the letter tiles
/// </summary>
public class QuartilesCracker
{
    // Contains common paths
    private QuartilePaths paths = new QuartilePaths(filesToBeModified: false);

    private string _currentDictionary = "quartiles_dictionary";

    // Path to the Dictionary file
    private string DictionaryPath => Path.Combine(paths.QuartilesCrackerDictFolder, $"{CurrentDictionary}.txt");

    // HashSet that contains all words for quick lookup
    private HashSet<string> dictionary = [];

    /// <summary>
    /// Gets and sets the maximum number of words chunks that can be used to form a word
    /// </summary>
    public int MaxChunks { get; set; } = 4;

    /// <summary>
    /// Gets and sets the maximum number of lines (rows) in a quartile game
    /// </summary>
    public int MaxLines { get; set; } = 5;

    /// <summary>
    /// Gets and Sets the filename (without file extension) for the dictionary to search for words and automatically reloads the dictionary if it is changed by the user
    /// </summary>
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

    /// <summary>
    /// Constructor for QuartilesCracker that initializes the current dictionary
    /// </summary>
    public QuartilesCracker()
    {
        LoadDictionary();
    }

    /// <summary>
    /// Finds all Quartile solutions from max sized chunk solutions through 1 size chunk solutions and including chunk mapping
    /// </summary>
    /// <param name="chunks">The letters found in a Quartiles game of size rows times columns</param>
    /// <returns>A tuple, with the first element containing all solutions and the second element containing the mapping for which chunks make up a solution</returns>
    public (HashSet<string> allSolutions, Dictionary<string, List<string>> solutionChunkMapping) QuartileSolverWithMapping(List<string> chunks)
    {
        VerifyChunks(chunks);
        HashSet<string> allSolutions = [];
        Dictionary<string, List<string>> solutionChunkMapping = [];

        for (int chunkSize = MaxChunks; chunkSize > 0; chunkSize--)
        {
            GetPermutations([], chunks, allSolutions, solutionChunkMapping, chunkSize);
        }

        return (allSolutions, solutionChunkMapping);
    }

    /// <summary>
    /// Finds all Quartile solutions from max sized chunk solutions through 1 size chunk solutions
    /// </summary>
    /// <param name="chunks">The letters found in a Quartiles game of size rows times columns</param>
    /// <returns>A set of all solutions found</returns>
    public HashSet<string> QuartileSolver(List<string> chunks)
    {
        VerifyChunks(chunks);
        HashSet<string> allSolutions = [];

        for (int chunkSize = MaxChunks; chunkSize > 0; chunkSize--)
        {
            GetPermutations([], chunks, allSolutions, chunkSize);
        }

        return allSolutions;

    }

    /// <summary>
    /// Generates all permutations of the chunks in the list and checks if they are valid words
    /// </summary>
    /// <param name="chunksOutOfList">Chunks used in current permutation. Initially empty</param>
    /// <param name="chunksInList">Chunks available to use for permutation. Initially full chunk list</param>
    /// <param name="solutions">Set where solutions are stored. Initially empty</param>
    /// <param name="solutionChunkMapping">Dictionary where solution-chunk mappings are stored. Initially empty/param>
    /// <param name="maxChunks">Maximum amount of chunks to use when permutating</param>
    public void GetPermutations(List<string> chunksOutOfList, List<string> chunksInList, HashSet<string> solutions, Dictionary<string, List<string>> solutionChunkMapping, int maxChunks)
    {
        if(chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if(dictionary.Contains(permutation))
            {
                solutions.Add(permutation);
                solutionChunkMapping[permutation] = [.. chunksOutOfList]; // Create copy to avoid storing reference and add
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
    /// Generates all permutations of the chunks in the list and checks if they are valid words
    /// </summary>
    /// <param name="chunksOutOfList">Chunks used in current permutation. Initially empty</param>
    /// <param name="chunksInList">Chunks available to use for permutation. Initially full chunk list</param>
    /// <param name="solutions">Set where solutions are stored. Initially empty</param>
    /// <param name="maxChunks">Maximum amount of chunks to use when permutating</param>
    public void GetPermutations(List<string> chunksOutOfList, List<string> chunksInList, HashSet<string> solutions, int maxChunks)
    {
        if (chunksOutOfList.Count == maxChunks)
        {
            // Join the chunks into one word
            string permutation = string.Join("", chunksOutOfList);

            if (dictionary.Contains(permutation))
            {
                solutions.Add(permutation);
            }

            return;
        }

        foreach (var chunk in chunksInList)
        {
            List<string> newChunkList = [.. chunksInList];
            newChunkList.Remove(chunk);
            List<string> newChunkOut = new(chunksOutOfList) { chunk };
            GetPermutations(newChunkOut, newChunkList, solutions, maxChunks);
        }
    }

    /// <summary>
    /// This method verifies that the chunk list is the correct size.
    /// </summary>
    /// <param name="chunks">The letters found in a Quartiles game</param>
    /// <exception cref="Exception">Thrown if the size of the list doesn't match board size</exception>
    public void VerifyChunks(List<string> chunks)
    {
        if(chunks.Count != MaxChunks * MaxLines)
        {
            throw new Exception("Chunk list does not match board size!");
        }
    }

    /// <summary>
    /// Loads new words into a new dictionary
    /// </summary>
    private void LoadDictionary()
    {
        paths.VerifyFile(DictionaryPath);
        dictionary = [.. File.ReadAllLines(DictionaryPath)];
    }
}