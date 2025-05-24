using Merger;
using Paths;

namespace Updater;

/// <summary>
/// Library for updating dictionaries, as well as known valid and invalid word lists, used in the QuartilesCracker application.
/// </summary>
public class DictionaryUpdater
{
    // Contains common paths
    private QuartilePaths paths = new QuartilePaths(filesToBeModified: true);

    // Backing fields
    private string _dictionaryFolder;
    private string _validWordsPath;
    private string _invalidWordsPath;

    /// <summary>
    /// Gets and sets a path to a folder containing the dictionaries to be updated. Verifies the directory exists before setting.
    /// </summary>
    public string DictionaryFolder 
    { 
        get => _dictionaryFolder; 
        set
        {
            paths.VerifyDirectory(value);
            _dictionaryFolder = value;
        }
    }

    /// <summary>
    /// Gets and sets a path to the valid words list which contains all known valid words for quartiles. Verifies the file exists before setting.
    /// </summary>
    public string ValidWordsPath
    {
        get => _validWordsPath;
        set
        {
            paths.VerifyFile(value);
            _validWordsPath = value;
        } 
    }

    /// <summary>
    /// Gets and sets a path to the invalid words list which contains all known invalid words for quartiles. Verifies the file exists before setting.
    /// </summary>
    public string InvalidWordsPath
    {
        get => _invalidWordsPath;

        set
        {
            paths.VerifyFile(value);
            _invalidWordsPath = value;
        }
    }

    /// <summary>
    /// Filename of the list containing known valid words without the file extension
    /// </summary>
    public string ValidWordsName { get; set; } = "known_valid_words";

    /// <summary>
    /// Filename of the list containing known invalid words without the file extension
    /// </summary>
    public string InvalidWordsName {  get; set; } = "known_invalid_words";

    /// <summary>
    /// Default constructor that sets DictionaryFolder to the QuartilesCracker MasterDictionary Folder
    /// </summary>
    public DictionaryUpdater()
    {
        DictionaryFolder = paths.QuartilesCrackerDictFolder;
        ValidWordsPath = Path.Combine(paths.DictionaryUpdaterListsFolder, ValidWordsName + ".txt");
        InvalidWordsPath = Path.Combine(paths.DictionaryUpdaterListsFolder, InvalidWordsName + ".txt");
    }

    /// <summary>
    /// Constructor that sets the DictionaryFolder to a given dictionary folder path
    /// </summary>
    /// <param name="dictionaryFolder">Path to a folder containing dictionaries to be updated</param>
    public DictionaryUpdater(string dictionaryFolder)
    {
        DictionaryFolder = dictionaryFolder;
        ValidWordsPath = Path.Combine(paths.DictionaryUpdaterListsFolder, ValidWordsName + ".txt");
        InvalidWordsPath = Path.Combine(paths.DictionaryUpdaterListsFolder, InvalidWordsName + ".txt");
    }

    /// <summary>
    /// Constructor that sets the known valid and invalid word lists path as well as the DictionaryFolder path
    /// </summary>
    /// <param name="listsFolder">Path to a folder containing the known valid and invalid word lists</param>
    /// <param name="dictionaryFolder">Path to a folder containing dictionaries to be updated</param>
    public DictionaryUpdater(string listsFolder, string dictionaryFolder)
    {
        DictionaryFolder = dictionaryFolder;
        ValidWordsPath = Path.Combine(listsFolder, ValidWordsName + ".txt");
        InvalidWordsPath = Path.Combine(listsFolder, InvalidWordsName + ".txt");
    }

    /// <summary>
    /// Given a word, add it to all used dictionaries
    /// </summary>
    /// <param name="word">Word to add to all dictionaries in the dictionary folder</param>
    public void AddToDictionaries(string word)
    {
        List<string> dictionaries = GetDictionaries();
        DictionaryMerger merger;

        foreach (string dictionary in dictionaries)
        {
            merger = new DictionaryMerger(DictionaryFolder, dictionary);
            merger.MergeDictionaries(word);
        }
    }

    /// <summary>
    /// Given a set of words, add them to all used dictionaries
    /// </summary>
    /// <param name="words">A set of words to add to all dictionaries in the dictionary folder</param>
    public void AddToDictionaries(HashSet<string> words)
    {
        List<string> dictionaries = GetDictionaries();
        DictionaryMerger merger;

        foreach (string dictionary in dictionaries)
        {
            merger = new DictionaryMerger(DictionaryFolder, dictionary);
            merger.MergeDictionaries(words);
        }
    }

    /// <summary>
    /// Given a word, remove it from all used dictionaries
    /// </summary>
    /// <param name="word">Word to remove from all dictionaries in the dictionary folder</param>
    public void RemoveFromDictionaries(string word)
    {
        List<string> dictionaries = GetDictionaries();
        DictionaryMerger merger;

        foreach (string dictionary in dictionaries)
        {
            merger = new DictionaryMerger(DictionaryFolder, dictionary, addToDictionary: false);
            merger.MergeDictionaries(word);
        }
    }

    /// <summary>
    /// Given a set of words, remove them from all used dictionaries
    /// </summary>
    /// <param name="words">Words to remove from all dictionaries in the dictionary folder</param>
    public void RemoveFromDictionaries(HashSet<string> words)
    {
        List<string> dictionaries = GetDictionaries();
        DictionaryMerger merger;

        foreach (string dictionary in dictionaries)
        {
            merger = new DictionaryMerger(DictionaryFolder, dictionary, addToDictionary: false);
            merger.MergeDictionaries(words);
        }
    }

    /// <summary>
    /// Given a known valid word, add it to the known valid words list.
    /// </summary>
    /// <param name="validWord">Known valid word in a quartiles' solution</param>
    public void AddToValidWords(string validWord)
    {
        var merger = new DictionaryMerger(ValidWordsPath);
        merger.MergeDictionaries(validWord);
    }

    /// <summary>
    /// Given a set of known valid words, add them to the known valid words list.
    /// </summary>
    /// <param name="validWords">A set of known valid words in a quartiles' solution</param>
    public void AddToValidWords(HashSet<string> validWords)
    {
        var merger = new DictionaryMerger(ValidWordsPath);
        merger.MergeDictionaries(validWords);
    }

    /// <summary>
    /// Given a word that is not part of a quartiles' solution, add it to the invalid words list.
    /// </summary>
    /// <param name="invalidWord">A word that is not part of a quartiles' solution</param>
    public void AddToInvalidWords(string invalidWord)
    {
        var merger = new DictionaryMerger(InvalidWordsPath);
        merger.MergeDictionaries(invalidWord);
    }

    /// <summary>
    /// Given a set of words that are not part of a quartiles' puzzle solution, add them to the invalid words list.
    /// </summary>
    /// <param name="invalidWords">A set of words that are not part of a quartiles' solution</param>
    public void AddToInvalidWords(HashSet<string> invalidWords)
    {
        var merger = new DictionaryMerger(InvalidWordsPath);
        merger.MergeDictionaries(invalidWords);
    }

    /// <summary>
    /// Given a known valid word, add it to all used dictionaries and the known valid words list.
    /// <para>
    /// Note: Most known valid words are in the master dictionary, so doing this operation can be unnecessary and slow.
    /// But, it is useful in the fact that it ensures all words in the known word list are in the master dictionary.
    /// </para>
    /// </summary>
    /// <param name="validWord">A known valid word in the quartile solution</param>
    public void AddUpdate(string validWord)
    {
        AddToDictionaries(validWord);
        AddToValidWords(validWord);
    }

    /// <summary>
    /// Given a set of known valid words, add them to all used dictionaries and the known words list.
    /// 
    /// <para>
    /// Note: Most known valid words are in the master dictionary, so doing this operation can be unnecessary and slow.
    /// But, it is useful in the fact that it ensures all words in the known valid word list are in the master dictionary.
    /// </para>
    /// </summary>
    /// <param name="validWords">A set of known word in a quartile solution</param>
    public void AddUpdate(HashSet<string> validWords)
    {
        AddToDictionaries(validWords);
        AddToValidWords(validWords);
    }

    /// <summary>
    /// Given a known invalid word, removes it from all used dictionaries and adds it to the invalid words list.
    /// </summary>
    /// <param name="invalidWord">A word that isn't a valid for a quartiles solution</param>
    public void RemoveUpdate(string invalidWord)
    {
        RemoveFromDictionaries(invalidWord);
        AddToInvalidWords(invalidWord);
    }

    /// <summary>
    /// Given a set of known invalid words, removes them from all used dictionaries and adds them to the invalid words list.
    /// </summary>
    /// <param name="invalidWords"></param>
    public void RemoveUpdate(HashSet<string> invalidWords)
    {
        RemoveFromDictionaries(invalidWords);
        AddToInvalidWords(invalidWords);
    }

    /// <summary>
    /// Given a set of all words and a set of valid words, removes all valid words from the set of all words to leave just the invalid words.
    /// </summary>
    /// <param name="allWords">A set of words that contains valid words but could contain extra (invalid words). Usually the result produced by QuartilesCracker</param>
    /// <param name="validWords">A set of words that are known to be the solution to a quartile. (Must contain ALL possible valid words so use trustworthy full solutions found online)</param>
    public void RemoveUpdate(HashSet<string> allWords, HashSet<string> validWords)
    {
        // This updates allWords by removing all words that are in validWords, leaving just the invalid words
        allWords.ExceptWith(validWords);
        RemoveUpdate(allWords);
    }

    /// <summary>
    /// Given a source path to only known valid words, reads all lines and calls AddUpdate
    /// </summary>
    /// <param name="sourcePath">Path to file containing only known valid words</param>
    public void AddUpdateFromFile(string sourcePath)
    {
        var merger = new DictionaryMerger();
        paths.VerifyFile(sourcePath);

        var sourceWords = new HashSet<string>(File.ReadAllLines(sourcePath));
        var safeSourceWords = merger.FilterValidWords(sourceWords);

        AddUpdate(safeSourceWords);
    }

    /// <summary>
    /// Verifies that the list of dictionaries is not empty.
    /// </summary>
    /// <param name="dictionaries">A list of dictionaries found in the dictionaryPath folder</param>
    /// <exception cref="ArgumentException">Thrown if there are no dictionaries in the given dictionary folder</exception>
    private void VerifyDictionaries(List<string> dictionaries)
    {
        if(dictionaries == null || dictionaries.Count == 0)
        {
            throw new ArgumentException("Dictionaries not found!");
        }
    }

    /// <summary>
    /// Gets the list of dictionaries in the dictionaryFolder.
    /// </summary>
    /// <returns>A list of dictionary file names (without the file extensions)</returns>
    private List<string> GetDictionaries()
    {
        List<string> dictionaries = Directory.GetFiles(DictionaryFolder, "*.txt")
                             .Select(Path.GetFileNameWithoutExtension)
                             .ToList();

        VerifyDictionaries(dictionaries);

        return dictionaries;
    }
}


