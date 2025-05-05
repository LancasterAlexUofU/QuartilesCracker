namespace DictionaryUpdater;

public class DictionaryUpdater
{
    public DictionaryUpdater()
    {

    }

    public void AddToDictionary(string word)
    {

    }
    public void AddToDictionary(List<string> words)
    {

    }

    public void RemoveFromDictionary(string word)
    {

    }

    public void RemoveFromDictionary(List<string> words)
    {

    }

    public void AddToKnownWords(string knownWord)
    {

    }

    public void AddToKnownWords(List<string> knownWords)
    {

    }
    public void AddToInvalidWords(string invalidWord)
    {

    }

    public void AddToInvalidWords(List<string> invalidWords)
    {

    }

    public void AddUpdate(string knownWord)
    {
        AddToDictionary(knownWord);
        AddToKnownWords(knownWord);
    }
    public void AddUpdate(List<string> knownWords)
    {
        AddToDictionary(knownWords);
        AddToKnownWords(knownWords);
    }

    public void RemoveUpdate(string invalidWord)
    {
        RemoveFromDictionary(invalidWord);
        AddToInvalidWords(invalidWord);
    }

    public void RemoveUpdate(List<string> knownWords, List<string> results)
    {
        // invalidWords = knownWord - results
        // RemoveFromDictionary(invalidWords);
        // AddToInvalidWords(invalidWords);
    }

}