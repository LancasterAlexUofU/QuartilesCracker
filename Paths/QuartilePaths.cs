namespace Paths
{
    /// <summary>
    /// Library that holds common file and folder paths throughout the project
    /// </summary>
    public class QuartilePaths
    {
        // Backing fields
        private string _chunkRoot;
        private string _dictionaryMergerRoot;
        private string _dictionaryUpdaterRoot;
        private string _quartilesAnswersRoot;
        private string _quartilesCrackerRoot;
        private string _quartilesRunnerRoot;
        private string _quartilesTestRoot;
        private string _quartilesToTextRoot;
        private string _solutionClickerRoot;

        private string _dictionaryMergerDictFolder;
        private string _dictionaryUpdaterListsFolder;
        private string _quartilesAnswersFolder;
        private string _quartilesCrackerDictFolder;
        private string _quartilesRunnerUpdatedDictFolder;
        private string _quartilesTestDictCopyFolder;
        private string _quartilesTestDictFolder;
        private string _quartilesTestListsCopyFolder;
        private string _quartilesTestListsFolder;
        private string _quartilesTestSourceFolder;
        private string _quartilesToTextChunksFolder;
        private string _quartilesToTextImagesFolder;
        private string _quartilesToTextTessdataBackupFolder;
        private string _quartilesToTextTessdataFolder;

        // > > > Project paths < < <
        // -------------------------

        /// <summary>
        /// Path to the solution directory
        /// </summary>
        public string SolutionRoot { get; private set; } = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\.."));

        /// <summary>
        /// Path to the folders when the solution is build (bin\Debug\netX.X). Used when files aren't being added or modified in library use
        /// </summary>
        public string BuildRoot { get; private set; } = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory));

        /// <summary>
        /// Path for the Chunk library
        /// </summary>
        public string ChunkRoot { get { VerifyDirectory(_chunkRoot); return _chunkRoot; } private set { _chunkRoot = value; } }

        /// <summary>
        /// Path for the DictionaryMerger library
        /// </summary>
        public string DictionaryMergerRoot { get { VerifyDirectory(_dictionaryMergerRoot); return _dictionaryMergerRoot; } private set { _dictionaryMergerRoot = value; } }

        /// <summary>
        /// Path for the DictionaryUpdater library
        /// </summary>
        public string DictionaryUpdaterRoot { get { VerifyDirectory(_dictionaryUpdaterRoot); return _dictionaryUpdaterRoot; } private set { _dictionaryUpdaterRoot = value; } }

        /// <summary>
        /// Path for the QuartilesAnswers class
        /// </summary>
        public string QuartilesAnswersRoot { get { VerifyDirectory(_quartilesAnswersRoot); return _quartilesAnswersRoot; } private set { _quartilesAnswersRoot = value; } }

        /// <summary>
        /// Path for the QuartilesCracker library
        /// </summary>
        public string QuartilesCrackerRoot { get { VerifyDirectory(_quartilesCrackerRoot); return _quartilesCrackerRoot; } private set { _quartilesCrackerRoot = value; } }

        /// <summary>
        /// Path for the QuartilesRunner class
        /// </summary>
        public string QuartilesRunnerRoot { get { VerifyDirectory(_quartilesRunnerRoot); return _quartilesRunnerRoot; } private set { _quartilesRunnerRoot = value; } }

        /// <summary>
        /// Path for the QuartilesTest suite
        /// </summary>
        public string QuartilesTestRoot { get { VerifyDirectory(_quartilesTestRoot); return _quartilesTestRoot; } private set { _quartilesTestRoot = value; } }

        /// <summary>
        /// Path for the QuartilesToText library
        /// </summary>
        public string QuartilesToTextRoot { get { VerifyDirectory(_quartilesToTextRoot); return _quartilesToTextRoot; } private set { _quartilesToTextRoot = value; } }

        /// <summary>
        /// Path for the SolutionClicker class
        /// </summary>
        public string SolutionClickerRoot { get { VerifyDirectory(_solutionClickerRoot); return _solutionClickerRoot; } private set { _solutionClickerRoot = value; } }

        // > > > Folders inside projects < < <
        // -----------------------------------

        /// <summary>
        /// Dictionary folder that contains a variety of different dictionaries
        /// </summary>
        public string DictionaryMergerDictFolder { get { VerifyDirectory(_dictionaryMergerDictFolder); return _dictionaryMergerDictFolder; } private set { _dictionaryMergerDictFolder = value; } }

        /// <summary>
        /// List folder that contains known valid and invalid word lists
        /// </summary>
        public string DictionaryUpdaterListsFolder { get { VerifyDirectory(_dictionaryUpdaterListsFolder); return _dictionaryUpdaterListsFolder; } private set { _dictionaryUpdaterListsFolder = value; } }

        /// <summary>
        /// Folder that contains solutions for past quartile games. Files are in the format "quartiles-answers-YYYY-MM-DD.png"
        /// </summary>
        public string QuartilesAnswersFolder { get { VerifyDirectory(_quartilesAnswersFolder); return _quartilesAnswersFolder; } private set { _quartilesAnswersFolder = value; } }

        /// <summary>
        /// Dictionary folder that contains "master" dictionaries used when solving quartiles
        /// </summary>
        public string QuartilesCrackerDictFolder { get { VerifyDirectory(_quartilesCrackerDictFolder); return _quartilesCrackerDictFolder; } private set { _quartilesCrackerDictFolder = value; } }

        /// <summary>
        /// Dictionary folder, primarily used for directing and testing program outputs
        /// </summary>
        public string QuartilesRunnerUpdatedDictFolder { get { VerifyDirectory(_quartilesRunnerUpdatedDictFolder); return _quartilesRunnerUpdatedDictFolder; } private set { _quartilesRunnerUpdatedDictFolder = value; } }

        /// <summary>
        /// Dictionary folder that contains copies of original dictionaries to reset after testing 
        /// </summary>
        public string QuartilesTestDictCopyFolder { get { VerifyDirectory(_quartilesTestDictCopyFolder); return _quartilesTestDictCopyFolder; } private set { _quartilesTestDictCopyFolder = value; } }

        /// <summary>
        /// Dictionary folder that contains dictionaries to be used during testing. Dictionaries in this folder will be overwritten at the end of each test.
        /// </summary>
        public string QuartilesTestDictFolder { get { VerifyDirectory(_quartilesTestDictFolder); return _quartilesTestDictFolder; } private set { _quartilesTestDictFolder = value; } }

        /// <summary>
        /// List folder that contains copies of original lists to reset after testing
        /// </summary>
        public string QuartilesTestListsCopyFolder { get { VerifyDirectory(_quartilesTestListsCopyFolder); return _quartilesTestListsCopyFolder; } private set { _quartilesTestListsCopyFolder = value; } }

        /// <summary>
        /// List folder that contains known valid and invalid words to be used during testing. Lists in this folder will be overwritten at the end of each test.
        /// </summary>
        public string QuartilesTestListsFolder { get { VerifyDirectory(_quartilesTestListsFolder); return _quartilesTestListsFolder; } private set { _quartilesTestListsFolder = value; } }

        /// <summary>
        /// List of words to be added (or not added) to dictionaries for testing purposes.
        /// </summary>
        public string QuartilesTestSourceFolder { get { VerifyDirectory(_quartilesTestSourceFolder); return _quartilesTestSourceFolder; } private set { _quartilesTestSourceFolder = value; } }

        /// <summary>
        /// List of Quartile games as their "chunk" formation. Files are in the form "quartiles-chunk-YYYY-MM-DD.png"
        /// </summary>
        public string QuartilesToTextChunksFolder { get { VerifyDirectory(_quartilesToTextChunksFolder); return _quartilesToTextChunksFolder; } private set { _quartilesToTextChunksFolder = value; } }

        /// <summary>
        /// Image Folder that contains pictures of Quartile games. Files are in the form "quartiles-YYYY-MM-DD.png"
        /// </summary>
        public string QuartilesToTextImagesFolder { get { VerifyDirectory(_quartilesToTextImagesFolder); return _quartilesToTextImagesFolder; } private set { _quartilesToTextImagesFolder = value; } }

        /// <summary>
        /// tessdata backup folder that contains data for OCR scanning. Contains models trained by ML and non-ML, so training sets can easily be switched out in the tessdata folder
        /// </summary>
        public string QuartilesToTextTessdataBackupFolder { get { VerifyDirectory(_quartilesToTextTessdataBackupFolder); return _quartilesToTextTessdataBackupFolder; } private set { _quartilesToTextTessdataBackupFolder = value; } }

        /// <summary>
        /// tessdata folder that contains data that is used by an OCR scanner to turn Quartiles into text
        /// </summary>
        public string QuartilesToTextTessdataFolder { get { VerifyDirectory(_quartilesToTextTessdataFolder); return _quartilesToTextTessdataFolder; } private set { _quartilesToTextTessdataFolder = value; } }

        /// <summary>
        /// Constructor that initializes roots and folders
        /// <para>
        /// If you are using programs that write to files (such as DictionaryUpdater, QuartilesAnswers, QuartilesToText, set libraryUse to false.
        /// Files that are written to in bin folders are deleted, so they need to be written to equivalent top-level folders. All top-level folders will be copied to equivalent build bins if modified.
        /// </para>
        /// </summary>
        /// <param name="libraryUse">If being used in a library format (where folders are found in the bin folder)</param>
        public QuartilePaths(bool libraryUse = true)
        {
            // If adding a new folder, make sure to add to both the BuildRoot and the local root

            ChunkRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "Chunk"));
            DictionaryMergerRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "DictionaryMerger"));
            DictionaryUpdaterRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "DictionaryUpdater"));
            QuartilesAnswersRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "QuartilesAnswers"));
            QuartilesCrackerRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "QuartilesCracker"));
            QuartilesRunnerRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "QuartilesRunner"));
            QuartilesTestRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "QuartilesTest"));
            QuartilesToTextRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "QuartilesToText"));
            SolutionClickerRoot = Path.GetFullPath(Path.Combine(SolutionRoot, "SolutionClicker"));

            if (libraryUse)
            {
                DictionaryMergerDictFolder = Path.Combine(BuildRoot, "Dictionaries");
                DictionaryUpdaterListsFolder = Path.Combine(BuildRoot, "Lists");
                QuartilesAnswersFolder = Path.Combine(BuildRoot, "QuartilesAnswers");
                QuartilesCrackerDictFolder = Path.Combine(BuildRoot, "MasterDictionaries");
                QuartilesTestDictCopyFolder = Path.Combine(BuildRoot, "TestDictionaryCopy");
                QuartilesTestDictFolder = Path.Combine(BuildRoot, "TestDictionary");
                QuartilesTestListsCopyFolder = Path.Combine(BuildRoot, "TestListsCopy");
                QuartilesTestListsFolder = Path.Combine(BuildRoot, "TestLists");
                QuartilesTestSourceFolder = Path.Combine(BuildRoot, "TestSource");
                QuartilesToTextChunksFolder = Path.Combine(BuildRoot, "QuartileChunks");
                QuartilesToTextImagesFolder = Path.Combine(BuildRoot, "QuartileImages");
                QuartilesToTextTessdataBackupFolder = Path.Combine(BuildRoot, "tessdataBackup");
                QuartilesToTextTessdataFolder = Path.Combine(BuildRoot, "tessdata");
            }

            else
            {
                DictionaryMergerDictFolder = Path.Combine(DictionaryMergerRoot, "Dictionaries");
                DictionaryUpdaterListsFolder = Path.Combine(DictionaryUpdaterRoot, "Lists");
                QuartilesAnswersFolder = Path.Combine(QuartilesAnswersRoot, "QuartilesAnswers");
                QuartilesCrackerDictFolder = Path.Combine(QuartilesCrackerRoot, "MasterDictionaries");
                QuartilesTestDictCopyFolder = Path.Combine(QuartilesTestRoot, "TestDictionaryCopy");
                QuartilesTestDictFolder = Path.Combine(QuartilesTestRoot, "TestDictionary");
                QuartilesTestListsCopyFolder = Path.Combine(QuartilesTestRoot, "TestListsCopy");
                QuartilesTestListsFolder = Path.Combine(QuartilesTestRoot, "TestLists");
                QuartilesTestSourceFolder = Path.Combine(QuartilesTestRoot, "TestSource");
                QuartilesToTextChunksFolder = Path.Combine(QuartilesToTextRoot, "QuartileChunks");
                QuartilesToTextImagesFolder = Path.Combine(QuartilesToTextRoot, "QuartileImages");
                QuartilesToTextTessdataBackupFolder = Path.Combine(QuartilesToTextRoot, "tessdataBackup");
                QuartilesToTextTessdataFolder = Path.Combine(QuartilesToTextRoot, "tessdata");
            }
        }

        /// <summary>
        /// Verifies that a given file exists at a given file path
        /// </summary>
        /// <param name="filePath">File path to verify. Should contain a file extension at the end</param>
        /// <exception cref="FileNotFoundException">Thrown when a given file path could not be found</exception>
        public void VerifyFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File path does not exist: {filePath}");
            }
        }

        /// <summary>
        /// Verifies that a given directory path exists
        /// </summary>
        /// <param name="directoryPath">Directory path to verify. Should not have file extension</param>
        /// <exception cref="DirectoryNotFoundException">Thrown when a given directory path could not be found</exception>
        public void VerifyDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory path does not exist: {directoryPath}");
            }
        }
    }
}
