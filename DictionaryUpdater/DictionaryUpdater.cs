using Merger;

namespace Updater;

public class DictionaryUpdater
{
    /// <summary>
    /// Path to the quartiles cracker dictionaries folder
    /// </summary>
    private string dictionaryFolder;

    /// <summary>
    /// Path to the folder that contains the known words and invalid words lists
    /// </summary>
    private string listsFolder;

    /// <summary>
    /// Path to the known words list which contains all known words for quartiles
    /// </summary>
    public string knownWordsPath;

    public string knownWordsName = "known_valid_words";

    /// <summary>
    /// Path to the invalid words list which contains all known invalid words for quartiles
    /// </summary>
    public string invalidWordsPath;

    public string invalidWordsName = "known_invalid_words";


    public DictionaryUpdater()
    {
        var merger = new DictionaryMerger();
        listsFolder = merger.dictionaryUpdaterListsFolder;
        dictionaryFolder = merger.quartilesCrackerDictFolder;

        knownWordsPath = Path.Combine(listsFolder, knownWordsName + ".txt");
        invalidWordsPath = Path.Combine(listsFolder, invalidWordsName + ".txt");

        merger.VerifyPath(knownWordsPath);
        merger.VerifyPath(invalidWordsPath);
    }

    public DictionaryUpdater(string dictionaryFolder)
    {
        var merger = new DictionaryMerger();
        listsFolder = merger.dictionaryUpdaterListsFolder;
        this.dictionaryFolder = dictionaryFolder;

        knownWordsPath = Path.Combine(listsFolder, knownWordsName + ".txt");
        invalidWordsPath = Path.Combine(listsFolder, invalidWordsName + ".txt");

        merger.VerifyPath(knownWordsPath);
        merger.VerifyPath(invalidWordsPath);
        merger.VerifyPath(dictionaryFolder);
    }

    public DictionaryUpdater(string listsFolder, string dictionaryFolder)
    {
        var merger = new DictionaryMerger();
        this.listsFolder = listsFolder;
        this.dictionaryFolder = dictionaryFolder;

        knownWordsPath = Path.Combine(listsFolder, knownWordsName + ".txt");
        invalidWordsPath = Path.Combine(listsFolder, invalidWordsName + ".txt");

        merger.VerifyPath(knownWordsPath);
        merger.VerifyPath(invalidWordsPath);
        merger.VerifyPath(listsFolder);
        merger.VerifyPath(dictionaryFolder);
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
            merger = new DictionaryMerger(dictionaryFolder, dictionary);
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
            merger = new DictionaryMerger(dictionaryFolder, dictionary);
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
            merger = new DictionaryMerger(dictionaryFolder, dictionary, addToDictionary: false);
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
            merger = new DictionaryMerger(dictionaryFolder, dictionary, addToDictionary: false);
            merger.MergeDictionaries(words);
        }
    }

    /// <summary>
    /// Given a known word, add it to the known words list.
    /// </summary>
    /// <param name="knownWord">Known word in a quartiles' solution</param>
    public void AddToKnownWords(string knownWord)
    {
        var merger = new DictionaryMerger(knownWordsPath);
        merger.MergeDictionaries(knownWord);
    }

    /// <summary>
    /// Given a set of known words, add them to the known words list.
    /// </summary>
    /// <param name="knownWords">A set of known words in a quartiles' solution</param>
    public void AddToKnownWords(HashSet<string> knownWords)
    {
        var merger = new DictionaryMerger(knownWordsPath);
        merger.MergeDictionaries(knownWords);
    }

    /// <summary>
    /// Given a word that is not part of a quartiles' solution, add it to the invalid words list.
    /// </summary>
    /// <param name="invalidWord">A word that is not part of a quartiles' solution</param>
    public void AddToInvalidWords(string invalidWord)
    {
        var merger = new DictionaryMerger(invalidWordsPath);
        merger.MergeDictionaries(invalidWord);
    }

    /// <summary>
    /// Given a set of words that are not part of a quartiles' puzzle solution, add them to the invalid words list.
    /// </summary>
    /// <param name="invalidWords">A set of words that are not part of a quartiles' solution</param>
    public void AddToInvalidWords(HashSet<string> invalidWords)
    {
        var merger = new DictionaryMerger(invalidWordsPath);
        merger.MergeDictionaries(invalidWords);
    }

    /// <summary>
    /// Given a known word, add it to all used dictionaries and the known words list.
    /// <para>
    /// Note: Most known words are in the master dictionary, so doing this operation can be unnecessary and slow.
    /// But, it is useful in the fact that it ensures all words in the known word list are in the master dictionary.
    /// </para>
    /// </summary>
    /// <param name="knownWord">A known word in the quartile solution</param>
    public void AddUpdate(string knownWord)
    {
        AddToDictionaries(knownWord);
        AddToKnownWords(knownWord);
    }

    /// <summary>
    /// Given a set of known valid words, add them to all used dictionaries and the known words list.
    /// 
    /// <para>
    /// Note: Most known words are in the master dictionary, so doing this operation can be unnecessary and slow.
    /// But, it is useful in the fact that it ensures all words in the known word list are in the master dictionary.
    /// </para>
    /// </summary>
    /// <param name="knownWords">A set of known word in a quartile solution</param>
    public void AddUpdate(HashSet<string> knownWords)
    {
        AddToDictionaries(knownWords);
        AddToKnownWords(knownWords);
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

    public void AddUpdateFromFile(string sourcePath)
    {
        var merger = new DictionaryMerger();

        merger.VerifyPath(sourcePath);
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
        List<string> dictionaries = Directory.GetFiles(dictionaryFolder, "*.txt")
                             .Select(Path.GetFileNameWithoutExtension)
                             .ToList();

        VerifyDictionaries(dictionaries);

        return dictionaries;
    }

    public static void Main(string[] args) { }
}


