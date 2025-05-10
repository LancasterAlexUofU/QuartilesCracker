using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Updater;
using Merger;
using System.IO;

namespace QuartilesTest
{
    [TestClass]
    public class DictionaryUpdaterTests
    {
        private DictionaryMerger paths = new DictionaryMerger();

        /// <summary>
        /// Tests that the known words and invalid words paths are valid
        /// </summary>
        [TestMethod]
        public void DictionaryUpdater_DefaultConstuctor_PathsAreValid()
        {
            var updater = new DictionaryUpdater();
        }

        /// <summary>
        /// Tests that the known words and invalid words paths are valid given a dictionary folder
        /// </summary>
        [TestMethod]
        public void DictionaryUpdater_ConstructorWithDictionaryFolder_PathsAreValid()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestDictFolder);
        }

        /// <summary>
        /// Tests that the known words and invalid words paths are valid given a dictionary folder and a list folder
        /// </summary>
        [TestMethod]
        public void DictionaryUpdater_ConstructorWithDictionaryAndListFolder_PathsAreValid()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
        }

        /// <summary>
        /// Tests that a given word is added to all dictionaries
        /// </summary>
        [TestMethod]
        public void AddToDictionaries_KnownValidWord_AddsToAllDictionariesSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestDictFolder, "*.txt");

            try
            {
                updater.AddToDictionaries("camo");

                foreach (var dictionary in dictFiles)
                {
                    var dict = new HashSet<string>(File.ReadAllLines(dictionary));
                    Assert.IsTrue(dict.Contains("camo"), $"Dictionary {dictionary} does not contain the word 'camo' after adding it.");
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that a set of known words is added to all dictionaries
        /// </summary>
        [TestMethod]
        public void AddToDictionaries_KnownValidWordSet_AddsToAllDictionariesSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestDictFolder, "*.txt");
            var knownValidWords = new HashSet<string> { "camo", "stuntwoman" };

            try
            {
                updater.AddToDictionaries(knownValidWords);

                foreach (var dictionary in dictFiles)
                {
                    var dict = new HashSet<string>(File.ReadAllLines(dictionary));

                    foreach (var word in knownValidWords)
                    {
                        Assert.IsTrue(dict.Contains(word), $"Dictionary {dictionary} does not contain the word '{word}' after adding it.");
                    }
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that a word is removed from all dictionaries
        /// </summary>
        [TestMethod]
        public void RemoveFromDictionaries_KnownWord_RemovesFromAllDictionariesSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestDictFolder, "*.txt");
            try
            {
                // Obviously apple is a valid word in this case, just checking it removes from all dictionaries
                updater.RemoveFromDictionaries("apple");

                foreach (var dictionary in dictFiles)
                {
                    var dict = new HashSet<string>(File.ReadAllLines(dictionary));
                    Assert.IsFalse(dict.Contains("apple"), $"Dictionary {dictionary} contains the word 'apple' after removing it.");
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that a set of words are removed from all dictionaries
        /// </summary>
        [TestMethod]
        public void RemoveFromDictionaries_KnownWords_RemovesFromAllDictionariesSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestDictFolder, "*.txt");
            var words = new HashSet<string> { "apple", "banana" };

            try
            {
                updater.RemoveFromDictionaries(words);

                foreach (var dictionary in dictFiles)
                {
                    var dict = new HashSet<string>(File.ReadAllLines(dictionary));

                    foreach (var word in words)
                    {
                        Assert.IsFalse(dict.Contains(word), $"Dictionary {dictionary} contains the word '{word}' after removing it.");
                    }
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that a word is added to the known words file
        /// </summary>
        [TestMethod]
        public void AddToKnownWords_KnownValidWord_AddsToFileSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestListsFolder, "*.txt");

            string knownValidWordsName = "known_valid_words";
            var knownValidWordsPath = Path.Combine(paths.quartilesTestListsFolder, knownValidWordsName + ".txt");

            try
            {
                updater.AddToKnownWords("camo");

                var knownWords = new HashSet<string>(File.ReadAllLines(knownValidWordsPath));
                Assert.IsTrue(knownWords.Contains("camo"), $"File {knownValidWordsPath} does not contain the word 'camo' after adding it.");
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that all words in a set are added to the known words file
        /// </summary>
        [TestMethod]
        public void AddToKnownWords_KnownValidWords_AddsToFileSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestListsFolder, "*.txt");

            string knownValidWordsName = "known_valid_words";
            var knownValidWordsPath = Path.Combine(paths.quartilesTestListsFolder, knownValidWordsName + ".txt");

            var knownValidWords = new HashSet<string> { "camo", "stuntwoman" };

            try
            {
                updater.AddToKnownWords(knownValidWords);

                var knownWords = new HashSet<string>(File.ReadAllLines(knownValidWordsPath));

                foreach (var word in knownValidWords)
                {
                    Assert.IsTrue(knownWords.Contains(word), $"File {knownValidWordsPath} does not contain the word '{word}' after adding it.");
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that a word is added to the known invalid words list
        /// </summary>
        [TestMethod]
        public void AddToInvalidWords_KnownInvalidWord_AddsToFileSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestListsFolder, "*.txt");

            string knownInvalidWordsName = "known_invalid_words";
            var knownInvalidWordsPath = Path.Combine(paths.quartilesTestListsFolder, knownInvalidWordsName + ".txt");

            try
            {
                updater.AddToInvalidWords("ic");

                var invalidWords = new HashSet<string>(File.ReadAllLines(knownInvalidWordsPath));
                Assert.IsTrue(invalidWords.Contains("ic"), $"File {knownInvalidWordsPath} does not contain the word 'ic' after adding it.");
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that a set of words are added to the known invalid words list
        /// </summary>
        [TestMethod]
        public void AddToInvalidWords_KnownInvalidWords_AddsToFileSuccess()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestListsFolder, "*.txt");

            string knownInvalidWordsName = "known_invalid_words";
            var knownInvalidWordsPath = Path.Combine(paths.quartilesTestListsFolder, knownInvalidWordsName + ".txt");

            var knownInvalidWords = new HashSet<string> { "ic", "abcdef" };

            try
            {
                updater.AddToInvalidWords(knownInvalidWords);
                var invalidWords = new HashSet<string>(File.ReadAllLines(knownInvalidWordsPath));

                foreach (var word in knownInvalidWords)
                {
                    Assert.IsTrue(invalidWords.Contains(word), $"File {knownInvalidWordsPath} does not contain the word '{word}' after adding it.");
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Tests that the test dictionaries do not contain valid words that are not part of the original dictionaries
        /// 
        /// <para>
        /// Since the test dictionaries are a copy of the original dictionaries, they should not contain any of the words that are added to the dictionaries later.
        /// </para>
        /// 
        /// <para>
        /// If this test fails, this indicates that a test isn't cleaning up by copying the original dictionaries back to the test dictionaries.
        /// </para>
        /// </summary>
        [TestMethod]
        public void TestDictionary_ContainsCertainValidWords_False()
        {
            var testWords = new HashSet<string> { "camo", "stuntwoman" };
            foreach (var file in Directory.GetFiles(paths.quartilesTestDictFolder, "*.txt"))
            {
                var dict = new HashSet<string>(File.ReadAllLines(file));

                foreach (var word in testWords)
                {
                    Assert.IsFalse(dict.Contains(word), $"Dictionary {file} contains the word {word} which should not be part of the original dictionary.");
                }
            }
        }


        /// <summary>
        /// Tests that the test lists do not contain certain valid and invalid words that are not part of the original lists
        /// 
        /// <para>
        /// Since the test lists are a copy of the original lists, they should not contain any of the words that are added to the lists later.
        /// </para>
        /// 
        /// <para>
        /// If this test fails, this indicates that a test isn't cleaning up by copying the original lists back to the test lists.
        /// </para>
        /// </summary>
        [TestMethod]
        public void TestLists_ContainsCertainValidWords_False()
        {
            var validTestWords = new HashSet<string> { "camo", "stuntwoman" };
            var invalidTestWords = new HashSet<string> { "ic", "abcdef", "germanic" };

            var knownValidWordsPath = Path.Combine(paths.quartilesTestListsFolder, "known_valid_words.txt");
            var knownInvalidWordsPath = Path.Combine(paths.quartilesTestListsFolder, "known_invalid_words.txt");

            var knownValidWords = new HashSet<string>(File.ReadAllLines(knownValidWordsPath));
            var knownInvalidWords = new HashSet<string>(File.ReadAllLines(knownInvalidWordsPath));

            foreach (var validWord in validTestWords)
            {
                Assert.IsFalse(knownValidWords.Contains(validWord), $"File {knownValidWordsPath} contains the word {validWord} which should not be in the original list.");
            }

            foreach (var invalidWord in invalidTestWords)
            {
                Assert.IsFalse(knownInvalidWords.Contains(invalidWord), $"File {knownInvalidWordsPath} contains the word {invalidWord} which should not be in the original list.");
            }
        }

        /// <summary>
        /// Tests that a set difference is done correctly and that all invalid words are added to the known invalid words list
        /// </summary>
        [TestMethod]
        public void RemoveUpdate_AllWordsAndValidWordsParam_ContainsAllInvalidWords()
        {
            var updater = new DictionaryUpdater(paths.quartilesTestListsFolder, paths.quartilesTestDictFolder);
            var dictFiles = Directory.GetFiles(paths.quartilesTestListsFolder, "*.txt");

            var allWords = new HashSet<string>(new[]
            {
                "gesticulate", "dilogical", "diminutive", "ecological", "stuntwoman", "caliculate", "rumormonger", "logical",
                "germinate", "germanate", "germanic", "digestive", "digestor", "digimon", "minorate", "ulminic", "stuntman",
                "callout", "caloric", "calicut", "calicate", "manicate", "gestate", "gestor", "gestic", "lout", "loger", "lodi",
                "logi", "local", "loman", "lorum", "loor", "utrum", "gerut", "gerdi", "gerate", "germin", "german", "germon",
                "digest", "dilo", "dint", "dimin", "digi", "dior", "dimon", "atelo", "minger", "mindi", "minor", "minic", "gilo",
                "giger", "gidi", "gical", "gior", "ulger", "ulate", "ulmin", "ulman", "stunt", "studi", "callo", "calor", "calic",
                "wont", "woul", "woman", "manger", "mandi", "mangi", "manul", "manor", "manic", "rumor", "orlo", "orate", "orman",
                "orrum", "oric", "monger", "monic", "gest", "lo", "nt", "ut", "ger", "di", "ive", "ate", "min", "eco", "gi", "ul",
                "stu", "cal", "wo", "man", "rum", "or", "mon", "ic"
            });

            var validWords = new HashSet<string> {
                "diminutive", "ecological", "gesticulate", "rumormonger", "stuntwoman",
                "callout", "caloric", "digestive", "germinate", "logical",
                "stuntman", "digest", "dint", "gestate", "local",
                "lout", "manger", "manic", "manor", "minor",
                "orate", "rumor", "stunt", "woman", "wont",
                "ate", "gi", "man", "rum", "or"
            };

            // Stores all invalid words in invalidWords by doing a set difference (allWords - validWords = invalidWords) 
            var invalidWords = new HashSet<string>(allWords);
            invalidWords.ExceptWith(validWords);

            try
            {
                updater.RemoveUpdate(allWords, validWords);
                var knownInvalidWordsPath = Path.Combine(paths.quartilesTestListsFolder, "known_invalid_words.txt");
                var invalidWordsFile = new HashSet<string>(File.ReadAllLines(knownInvalidWordsPath));

                foreach (var word in invalidWords)
                {
                    Assert.IsTrue(invalidWordsFile.Contains(word), $"File {knownInvalidWordsPath} does not contain the word '{word}' after adding it.");
                }
            }

            finally
            {
                DictionaryCleanup();
            }
        }

        /// <summary>
        /// Reinstates the original dictionaries and lists by copying them back from the backup folder
        /// </summary>
        private void DictionaryCleanup()
        {
            var dictFiles = Directory.GetFiles(paths.quartilesTestDictFolder, "*.txt");
            var listFiles = Directory.GetFiles(paths.quartilesTestListsFolder, "*.txt");

            // Clean up the test by copying the original dictionaries back
            foreach (var dictionaryPath in dictFiles)
            {
                var fileName = Path.GetFileName(dictionaryPath);
                var dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, fileName);

                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }

            // Copy the original valid and invalid words lists back
            foreach (var listPath in listFiles)
            {
                var listName = Path.GetFileName(listPath);
                var listCopyPath = Path.Combine(paths.quartilesTestListsCopyFolder, listName);

                File.Copy(listCopyPath, listPath, overwrite: true);
            }
        }
    }
}