using System.Text.RegularExpressions;
using Paths;

namespace Merger;
/// <summary>
/// This program merges two dictionaries by adding (or removing) words from the source dictionary to the destination dictionary.
/// 
/// <para>
/// The source dictionary will not be modified, but the destination dictionary will be updated with new words.
/// </para>
/// 
/// <para>
/// Only new words from the source dictionary will be added to the destination dictionary and will not have duplicates.
/// </para>
/// 
/// </summary>
public class DictionaryMerger
{
    // Backing fields
    private string _sourcePath;
    private string _destinationPath;

    /// <summary>
    /// Gets and sets the path to where the source dictionary is saved. Verifies that the path is valid before setting.
    /// 
    /// <para>
    /// This is not the folder where the source dictionary is located, but the path to the file itself.
    /// </para>
    /// </summary>
    public string SourcePath
    {
        get => _sourcePath;
        private set
        {
            paths.VerifyFile(value);
            _sourcePath = value;
        }
    }

    /// <summary>
    /// Gets and sets the path to where the destination dictionary is saved. Verifies that the path is valid before setting.
    /// 
    /// <para>
    /// This is not the folder where the destination dictionary is located, but the path to the file itself.
    /// </para>
    /// </summary>
    public string DestinationPath
    { 
        get => _destinationPath; 
        private set
        {
            paths.VerifyFile(value);
            _destinationPath = value;
        }
    }

    /// <summary>
    /// Gets and sets a boolean to determine whether to add or remove words from the destination dictionary. True adds and false removes.
    /// </summary>
    public bool addToDictionary { get; set; }

    // Contains common paths
    private QuartilePaths paths = new QuartilePaths(libraryUse: false);

    /// <summary>
    /// Default constructor for DictionaryMerger
    /// </summary>
    public DictionaryMerger() { }

    /// <summary>
    /// Constructor that takes in a destination dictionary path
    /// </summary>
    /// <param name="dictionaryPath">Path to dictionary to be modified</param>
    /// <param name="addToDictionary">Adds to dictionary if true, removes if false</param>
    public DictionaryMerger(string dictionaryPath, bool addToDictionary = true)
    {
        DestinationPath = dictionaryPath;
        this.addToDictionary = addToDictionary;
    }

    /// <summary>
    /// Constructor that takes in the destination folder where a dictionary is located, along with the dictionary filename located inside the folder
    /// </summary>
    /// <param name="destinationFolder">Path to the folder containing the dictionary to be modified</param>
    /// <param name="dictionaryName">Filename of the dictionary without file extensions</param>
    /// <param name="addToDictionary">Adds to dictionary if true, removes if false</param>
    public DictionaryMerger(string destinationFolder, string dictionaryName, bool addToDictionary = true)
    {
        DestinationPath = Path.Combine(destinationFolder, dictionaryName + ".txt");
        this.addToDictionary = addToDictionary;
    }

    /// <summary>
    /// Constructor that takes in a source folder, where a dictionary to be read is located along with its filename, and a destination folder, where a dictionary to be written to is located along with its filename.
    /// </summary>
    /// <param name="sourceFolder">Path to the folder containing the dictionary to be read from</param>
    /// <param name="sourceName">Filename of the source dictionary without file extensions</param>
    /// <param name="destinationFolder">Path to the folder containing the dictionary to be modified</param>
    /// <param name="destinationName">Filename of the dictionary without file extensions</param>
    /// <param name="addToDictionary">Adds to dictionary if true, removes if false</param>
    public DictionaryMerger(string sourceFolder, string sourceName, string destinationFolder, string destinationName, bool addToDictionary = true)
    {
        SourcePath = Path.Combine(sourceFolder, sourceName + ".txt");
        DestinationPath = Path.Combine(destinationFolder, destinationName + ".txt");
        this.addToDictionary = addToDictionary;
    }

    /// <summary>
    /// Merges the source dictionary with the destination dictionary
    /// </summary>
    public void MergeDictionaries()
    {
        if (SourcePath == null)
        {
            throw new FileNotFoundException("Cannot merge without source dictionary. Please use a DictionaryMerger constructor that takes in a source file or use a different method if you are not using a source file.");
        }

        var sourceWords = new HashSet<string>(File.ReadAllLines(SourcePath));
        MergeWithDestination(sourceWords, addToDictionary);
    }

    /// <summary>
    /// Merges a word with the destination dictionary
    /// </summary>
    /// <param name="sourceWord">The word to merge with the destination dictionary</param>
    public void MergeDictionaries(string sourceWord)
    {
        var sourceWords = new HashSet<string> { sourceWord };
        MergeWithDestination(sourceWords, addToDictionary);
    }

    /// <summary>
    /// Merges a set of words with the destination dictionary
    /// </summary>
    /// <param name="sourceWords">A set of words to merge with the destination dictionary</param>
    public void MergeDictionaries(HashSet<string> sourceWords)
    {
        MergeWithDestination(sourceWords, addToDictionary);
    }

    /// <summary>
    /// Given a set of words, merges them with the destination dictionary
    /// </summary>
    /// <param name="sourceWords">Set of words to merge with destination dictionary</param>
    /// <param name="addToDictionary">When true, adds sourceWords to dictionary. When false, removes words from dictionary</param>
    private void MergeWithDestination(HashSet<string> sourceWords, bool addToDictionary)
    {
        try
        {
            var destinationWords = new HashSet<string>(File.ReadAllLines(DestinationPath));
            var safeSourceWords = FilterValidWords(sourceWords);

            // Merge both dictionaries — new words will be added at the end of destinationWords
            if (addToDictionary)
            {
                destinationWords.UnionWith(safeSourceWords);
            }

            else
            {
                destinationWords.ExceptWith(safeSourceWords);
            }

            // Sorts alphabetically and writes back to destination dictionary — source remains the same
            var sortedWords = destinationWords.OrderBy(word => word).ToList();
            WriteAllLines(DestinationPath, sortedWords);
        }

        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    /// <summary>
    /// Filters out invalid words from a set of words
    /// </summary>
    /// <param name="words">Set of words to filter out, usually sourceWords from MergeWithDestination</param>
    /// <returns>A set of words guaranteed to consist of lowercase letters with no non-word characters</returns>
    public HashSet<string> FilterValidWords(HashSet<string> words)
    {
        var regex = new Regex("^[a-zA-Z]+$");
        var result = new HashSet<string>();

        foreach (var word in words)
        {
            var trimmed = word.Trim();
            if (regex.IsMatch(trimmed))
            {
                result.Add(trimmed.ToLower());
            }
        }

        return result;
    }

    /// <summary>
    /// Writes all words line by line to the destination path, and importantly, has no extra newline at the end
    /// </summary>
    /// <param name="destinationPath">Dictionary path to be written to</param>
    /// <param name="words">Words to be written</param>
    private void WriteAllLines(string destinationPath, List<string> words)
    {
        using var writer = new StreamWriter(destinationPath);

        for (int i = 0; i < words.Count; i++)
        {
            if (i < words.Count - 1)
            {
                writer.WriteLine(words.ElementAt(i));
            }

            else
            {
                writer.Write(words.ElementAt(i)); // no newline
            }
        }
    }
}