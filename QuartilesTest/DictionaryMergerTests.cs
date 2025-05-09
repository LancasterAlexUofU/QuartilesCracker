using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Merger;

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
        /// Tests that a merger which never took in a source path throws an exception when using the parameterless merge call that reads from the source path.
        /// </summary>
        [TestMethod]
        public void MergeDictionaries_NoParam_ThrowsException()
        {
            var paths = new DictionaryMerger();
            string dictionaryPath = Path.Combine(paths.quartilesCrackerDictFolder, "quartiles_dictionary.txt");

            var merger = new DictionaryMerger(dictionaryPath);

            Assert.ThrowsException<Exception>(() => merger.MergeDictionaries());
        }
    }
}