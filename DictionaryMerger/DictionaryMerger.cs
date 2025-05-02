using System.Text.RegularExpressions;

/**
 * This program merges two dictionaries by adding words from the source dictionary to the destination dictionary.
 * 
 * To use, set the source and destination dictionary names in the code.
 * 
 * - The source dictionary will not be modified, but the destination dictionary will be updated with new words.
 * - Only new words from the source dictionary will be added to the destination dictionary and will not have duplicates.
 */
class ListMerger
{
    static void Main(string[] args)
    {
        // Go back 3 levels from bin\debug\netX.Y to save the file merge in the project folder, not the debug folder
        string dictionaryMergerRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));

        // Go back 1 level to solution from dictionary project, then go into QuartilesCracker project
        // This is so a a copy can be saved in DictionaryMerger and QuartileCracker
        string quartilesCrackerRoot = Path.GetFullPath(Path.Combine(dictionaryMergerRoot, @"..\QuartilesCracker"));

        // Source Dictionary — Might have word destination dictionary doesn't. Won't be modified
        string sourceDictionary = "wordscapesDictionary";

        // Destination Dictionary — Likely the "master" dictionary. Will be modified
        string destinationDictionary = "quartiles_dictionary";
        
        string sourcePathMerger = Path.Combine(dictionaryMergerRoot, "Dictionaries", sourceDictionary + ".txt");
        string destinationPathMerger = Path.Combine(dictionaryMergerRoot, "Dictionaries", destinationDictionary + ".txt");

        // Ensure that the destination dictionary exists with the same name as the one in DictionaryMerger in the QuartilesCracker Dictionary folder
        string destinationPathQuartile = Path.Combine(quartilesCrackerRoot, "Dictionaries", destinationDictionary + ".txt"); 
        

        if (!File.Exists(sourcePathMerger) || !File.Exists(destinationPathMerger) || !File.Exists(destinationPathQuartile))
        {
            Console.WriteLine("One or more files do not exist.");
            return;
        }

        try
        {
            var sourceWords = new HashSet<string>(File.ReadAllLines(sourcePathMerger));
            var destinationWords = new HashSet<string>(File.ReadAllLines(destinationPathMerger));

            var regex = new Regex("^[a-zA-Z]+$"); // matches only words made of letters, no spaces or special characters
            var safeSourceWords = new HashSet<string>();

            // Excludes any words that contain non-letter characters to be unioned 
            foreach(var word in sourceWords)
            {
                // Remove any trailing white space:
                string trimmedWord = word.Trim();

                if(regex.IsMatch(trimmedWord))
                {
                    safeSourceWords.Add(trimmedWord.ToLower());
                }
            }

            // Merge both dictionaries — new words will be added at the end of destinationWords
            destinationWords.UnionWith(safeSourceWords);

            // Sorts alphabetically and writes back to destination dictionary — source remains the same
            var sortedWords = destinationWords.OrderBy(word => word).ToList();
            File.WriteAllLines(destinationPathMerger, sortedWords);
            File.WriteAllLines(destinationPathQuartile, sortedWords);
            Console.WriteLine("Merge complete. Destination file updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
