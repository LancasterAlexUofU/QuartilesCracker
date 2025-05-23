using Quartiles;
using Updater;

class Program
{
    static void Main(string[] args)
    {
        var solver = new QuartilesCracker();
        solver.CurrentDictionary = "quartiles_dictionary_updated";

        // Solve all chunks
        // Foreach chunk file
        // For all results 

        string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
        string quartilesRunnerRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesRunner"));
        string quartilesToTextRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesToText"));

        string quartileChunksFolder = Path.Combine(quartilesToTextRoot, "QuartileChunks");
        string quartilesAnswersRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesAnswers"));
        string quartilesAnswersFolder = Path.Combine(quartilesAnswersRoot, "QuartilesAnswers");

        string dictionaryFolder = Path.Combine(quartilesRunnerRoot, "UpdatedDictionaries");

        var updater = new DictionaryUpdater(dictionaryFolder);

        string[] chunkFiles = Directory.GetFiles(quartileChunksFolder);

        foreach (var chunkFile in chunkFiles)
        {
            string chunkFileName = Path.GetFileName(chunkFile);
            string datePart = chunkFileName.Substring("quartiles-chunk-".Length, "YYYY-MM-DD".Length);

            string answerFileName = $"quartiles-answers-{datePart}.txt";
            string answerFilePath = Path.Combine(quartilesAnswersFolder, answerFileName);

            if (File.Exists(answerFilePath))
            {
                Console.WriteLine($"Updating {chunkFileName}");
                List<string> chunks = new List<string>(File.ReadAllLines(chunkFile));
                HashSet<string> correctAnswers = new HashSet<string>(File.ReadAllLines(answerFilePath));
                List<string> allSolutions = new List<string>();

                (allSolutions, var notinuse) = solver.QuartileSolver(chunks);
                updater.RemoveUpdate(allSolutions.ToHashSet(), correctAnswers);
            }
        }
    }
}