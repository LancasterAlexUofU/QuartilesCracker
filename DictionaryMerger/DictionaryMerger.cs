using System.Text.RegularExpressions;

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
    /// <summary>
    /// Path to where the source dictionary is saved
    /// 
    /// <para>
    /// This is not the folder where the source dictionary is located, but the path to the file itself.
    /// </para>
    /// </summary>
    public string sourcePath;

    /// <summary>
    /// Path to where the destination dictionary is saved
    /// 
    /// <para>
    /// This is not the folder where the destination dictionary is located, but the path to the file itself.
    /// </para>
    /// </summary>
    public string destinationPath;

    /// <summary>
    /// Boolean to determine whether to add or remove words from the destination dictionary
    /// </summary>
    public bool addToDictionary;

    /// <summary>
    /// Holds the path to the current project root.
    /// 
    /// <para>
    /// Goes back 3 levels from bin\debug\netX.Y to get to project folder
    /// </para>
    /// </summary>
    public string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));

    // Common paths
    public string dictionaryMergerDictFolder;
    public string dictionaryUpdaterListsFolder;
    public string quartilesCrackerDictFolder;
    public string quartilesTestDictFolder;
    public string quartilesTestDictCopyFolder;
    public string quartilesTestSourceFolder;
    public string quartilesTestListsFolder;
    public string quartilesTestListsCopyFolder;

    public DictionaryMerger()
    {
        InitializeCommonFolderPaths();
    }

    public DictionaryMerger(string dictionaryPath, bool addToDictionary = true)
    {
        InitializeCommonFolderPaths();

        destinationPath = dictionaryPath;
        this.addToDictionary = addToDictionary;

        VerifyPath(destinationPath);
    }

    public DictionaryMerger(string destinationFolder, string dictionaryName, bool addToDictionary = true)
    {
        InitializeCommonFolderPaths();

        destinationPath = Path.Combine(destinationFolder, dictionaryName + ".txt");
        this.addToDictionary = addToDictionary;

        VerifyPath(destinationPath);
    }

    public DictionaryMerger(string sourceFolder, string sourceName, string destinationFolder, string destinationName, bool addToDictionary = true)
    {
        InitializeCommonFolderPaths();

        sourcePath = Path.Combine(sourceFolder, sourceName + ".txt");
        destinationPath = Path.Combine(destinationFolder, destinationName + ".txt");
        this.addToDictionary = addToDictionary;

        VerifyPath(sourcePath);
        VerifyPath(destinationPath);
    }

    /// <summary>
    /// Initializes common source and destination folder paths for the dictionaries
    /// </summary>
    private void InitializeCommonFolderPaths()
    {
        // Ensure GetFullPath for ../ to work correctly
        var dictionaryMergerRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\DictionaryMerger"));
        var dictionaryUpdaterRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\DictionaryUpdater"));
        var quartilesCrackerRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesCracker"));
        var quartilesTestRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesTest"));

        dictionaryMergerDictFolder = Path.Combine(dictionaryMergerRoot, "Dictionaries");
        dictionaryUpdaterListsFolder = Path.Combine(dictionaryUpdaterRoot, "Lists");
        quartilesCrackerDictFolder = Path.Combine(quartilesCrackerRoot, "Dictionaries");
        quartilesTestDictFolder = Path.Combine(quartilesTestRoot, "TestDictionary");
        quartilesTestDictCopyFolder = Path.Combine(quartilesTestRoot, "TestDictionaryCopy");
        quartilesTestSourceFolder = Path.Combine(quartilesTestRoot, "TestSource");
        quartilesTestListsFolder = Path.Combine(quartilesTestRoot, "TestLists");
        quartilesTestListsCopyFolder = Path.Combine(quartilesTestRoot, "TestListsCopy");

        VerifyPath(dictionaryMergerDictFolder);
        VerifyPath(dictionaryUpdaterListsFolder);
        VerifyPath(quartilesCrackerDictFolder);
        VerifyPath(quartilesTestDictFolder);
        VerifyPath(quartilesTestDictCopyFolder);
        VerifyPath(quartilesTestSourceFolder);
        VerifyPath(quartilesTestListsFolder);
        VerifyPath(quartilesTestListsCopyFolder);
    }

    /// <summary>
    /// Merges the source dictionary with the destination dictionary
    /// </summary>
    public void MergeDictionaries()
    {
        VerifyPath(sourcePath);

        var sourceWords = new HashSet<string>(File.ReadAllLines(sourcePath));
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
            var destinationWords = new HashSet<string>(File.ReadAllLines(destinationPath));
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
            File.WriteAllLines(destinationPath, sortedWords);
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

    public void VerifyPath(string filePath)
    {
        if (!Directory.Exists(filePath) && !File.Exists(filePath))
        {
            throw new Exception($"File path does not exist: {filePath}");
        }
    }

    public static void Main(string[] args) { }
}