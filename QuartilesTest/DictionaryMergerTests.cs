using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Merger;
using System.Collections;

namespace QuartilesTest
{
    [TestClass]
    public class DictionaryMergerTests
    {
        /// <summary>
        /// Tests that the paths for the dictionary merger are valid.
        /// </summary>
        [TestMethod]
        public void DictionaryMerger_DefaultConstructor_PathsAreValid()
        {
            var merger = new DictionaryMerger();
        }

        /// <summary>
        /// Tests that the destination path is valid when passed as a parameter.
        /// </summary>
        [TestMethod]
        public void DictionaryMerger_DictPathConstructor_PathsAreValid()
        {
            var paths = new DictionaryMerger();
            string dictionaryPath = Path.Combine(paths.quartilesCrackerDictFolder, "quartiles_dictionary.txt");

            var merger = new DictionaryMerger(dictionaryPath);
        }

        /// <summary>
        /// Tests that a folder and a dictionary name create a valid path when passed as parameters.
        /// </summary>
        [TestMethod]
        public void DictionaryMerger_DictNameConstructor_PathsAreValid()
        {
            var paths = new DictionaryMerger();
            var merger = new DictionaryMerger(paths.quartilesCrackerDictFolder, "quartiles_dictionary");
        }

        /// <summary>
        /// Tests that a source folder and a destination folder create valid paths when passed as parameters.
        /// </summary>
        [TestMethod]
        public void DictionaryMerger_SourceDestConstructor_PathsAreValid()
        {
            var paths = new DictionaryMerger();
            var merger = new DictionaryMerger(paths.dictionaryUpdaterListsFolder, "known_valid_words", paths.dictionaryMergerDictFolder, "quartiles_dictionary");
        }

        /// <summary>
        /// Tests that the original test dictionary does not contain the valid words "camo" and "stuntwoman"
        /// 
        /// <para>
        /// Since the test dictionary is a copy of the original dictionary, it should not contain any of the words that are added to the dictionary later.
        /// </para>
        /// 
        /// <para>
        /// If this test fails, this indicates that a test isn't cleaning up by copying the original dictionary back to the test dictionary.
        /// </para>
        /// </summary>
        [TestMethod]
        public void TestDictionary_ContainsCertainValidWords_False()
        {
            var paths = new DictionaryMerger();
            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");

            var dictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));
            Assert.IsFalse(dictionary.Contains("camo"), "Camo should not be part of the original dictionary");
            Assert.IsFalse(dictionary.Contains("stuntwoman"), "Stuntwoman should not be part of the original dictionary");

            // Ensures that the test dictionary contains the word "apple" which is a valid word (and is removed in a later test)
            Assert.IsTrue(dictionary.Contains("apple"), "Apple should be part of the original dictionary");
        }

        /// <summary>
        /// Tests that a merger which never took in a source path throws an exception when using the parameterless merge call that reads from the source path.
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_NoParam_ThrowsException()
        {
            var paths = new DictionaryMerger();
            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");
            string dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, dictionaryName + ".txt");

            var merger = new DictionaryMerger(dictionaryPath);

            try
            {
                Assert.ThrowsException<Exception>(() => merger.MergeDictionaries());
            }

            finally
            {
                // Clean up the test by copying the original dictionary back (shouldn't really need, but here just in case)
                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }
        }

        /// <summary>
        /// Tests that no duplicate words are present in the merged dictionary.
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_NoParamAdd_NoDuplicatesSuccess()
        {
            var paths = new DictionaryMerger();

            string sourceName = "source";
            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");
            string dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, dictionaryName + ".txt");
            var seen = new HashSet<string>();

            var merger = new DictionaryMerger(paths.quartilesTestSourceFolder, sourceName, paths.quartilesTestDictFolder, dictionaryName);

            try
            {
                merger.MergeDictionaries();
                string[] mergedDictionary = File.ReadAllLines(dictionaryPath);

                // Adds all words from a list to a set and adds duplicates when adding to the set fails
                var duplicates = mergedDictionary.Where(word => !seen.Add(word)).ToList();

                Assert.IsTrue(duplicates.Count == 0);
            }

            finally
            {
                // Clean up the test by copying the original dictionary back
                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }
        }

        /// <summary>
        /// Tests that merging from a source file to a destination file works correctly.
        /// 
        /// <para>
        /// Ensures only valid non-duplicate words are added, that invalid words weren't added, and that the dictionary size after merging is correct.
        /// </para>
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_NoParamAdd_MergeSuccess()
        {
            var paths = new DictionaryMerger();

            string sourceName = "source";
            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");
            string dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, dictionaryName + ".txt");

            var originalDictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));

            var merger = new DictionaryMerger(paths.quartilesTestSourceFolder, sourceName, paths.quartilesTestDictFolder, dictionaryName);

            try
            {
                merger.MergeDictionaries();

                var mergedDictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));

                /**
                 * source.txt contains the following words:
                 * camo
                 * stuntwoman
                 * dontadd wordswithspaces
                 * hyphens-dontcount
                 * duplicate
                 * 
                 * Of these words, only "camo" and "stuntwoman" should be added to the dictionary since they are not already present and are valid words.
                 */

                // Only these words should be added to the dictionary
                Assert.IsTrue(mergedDictionary.Contains("camo"), "Camo was not added to the dictionary");
                Assert.IsTrue(mergedDictionary.Contains("stuntwoman"), "Stuntwoman was not added to the dictionary");

                // None of these words should be added to the dictionary
                Assert.IsFalse(mergedDictionary.Contains("dontadd"), "dontadd was added to the dictionary");
                Assert.IsFalse(mergedDictionary.Contains("wordswithspaces"), "wordswithspaces was added to the dictionary");
                Assert.IsFalse(mergedDictionary.Contains("dontadd wordswithspaces"), "dontadd wordswithspaces was added to the dictionary");
                Assert.IsFalse(mergedDictionary.Contains("hyphens-dontcount"), "hyphens-dontcount was added to the dictionary");

                // Only 2 words should be added to the dictionary
                Assert.AreEqual(mergedDictionary.Count, originalDictionary.Count + 2, "The dictionary count is not correct after the merge");
            }

            finally
            {
                // Clean up the test by copying the original dictionary back
                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }
        }

        /// <summary>
        /// Tests that merging a single word into the dictionary works correctly.
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_SourceStringParamAdd_MergeSuccess()
        {
            var paths = new DictionaryMerger();

            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");
            string dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, dictionaryName + ".txt");

            var merger = new DictionaryMerger(dictionaryPath);

            try
            {
                merger.MergeDictionaries("camo");

                var mergedDictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));
                Assert.IsTrue(mergedDictionary.Contains("camo"), "Camo was not added to the dictionary");
            }

            finally
            {
                // Clean up the test by copying the original dictionary back
                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }
        }

        /// <summary>
        /// Tests that merging a set of words into the dictionary works correctly.
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_SourceSetParamAdd_MergeSuccess()
        {
            var paths = new DictionaryMerger();

            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");
            string dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, dictionaryName + ".txt");

            var merger = new DictionaryMerger(dictionaryPath);
            var sourceWords = new HashSet<string> { "camo", "stuntwoman" };

            try
            {
                merger.MergeDictionaries(sourceWords);

                var mergedDictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));
                Assert.IsTrue(mergedDictionary.Contains("camo"), "Camo was not added to the dictionary");
                Assert.IsTrue(mergedDictionary.Contains("stuntwoman"), "Stuntwoman was not added to the dictionary");
            }

            finally
            {
                // Clean up the test by copying the original dictionary back
                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }
        }

        /// <summary>
        /// Tests that merging a set of words to be removed from the dictionary works correctly.
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_SourceSetParamRemove_MergeSuccess()
        {
            var paths = new DictionaryMerger();
            string dictionaryName = "2of12";
            string dictionaryPath = Path.Combine(paths.quartilesTestDictFolder, dictionaryName + ".txt");
            string dictionaryCopyPath = Path.Combine(paths.quartilesTestDictCopyFolder, dictionaryName + ".txt");

            var originalDictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));

            var merger = new DictionaryMerger(dictionaryPath, addToDictionary: false);

            // Should only remove the words "apple" and "banana" and ignore the rest
            var sourceWords = new HashSet<string> { "apple", "banana", "camo", "notaword", "12345" };

            try
            {
                merger.MergeDictionaries(sourceWords);
                var mergedDictionary = new HashSet<string>(File.ReadAllLines(dictionaryPath));

                Assert.IsFalse(mergedDictionary.Contains("apple"), "Apple was not removed from the dictionary");
                Assert.IsFalse(mergedDictionary.Contains("banana"), "Banana was not removed from the dictionary");
                Assert.AreEqual(mergedDictionary.Count(), originalDictionary.Count() - 2);
            }

            finally
            {
                // Clean up the test by copying the original dictionary back
                File.Copy(dictionaryCopyPath, dictionaryPath, overwrite: true);
            }
        }
    }
}