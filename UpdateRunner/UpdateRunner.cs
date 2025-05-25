using Updater;
using Paths;
using Answers;
using ChunkWrite;
using Quartiles;

public class UpdateRunner
{
    DictionaryUpdater updater = new DictionaryUpdater();
    QuartilePaths paths = new QuartilePaths(true);
    ChunkWriter chunkWriter = new ChunkWriter();
    QuartilesCracker solver = new QuartilesCracker();

    public UpdateRunner() { }
    

    public static void Main(string[] args)
    {
        UpdateRunner runner = new UpdateRunner();

        //runner.AddAllValidWords();
        runner.AddAllChunks();
        runner.AllChunkAnswers();
    }

    public void AddAllChunks()
    {
        Console.WriteLine("Adding all chunks...");
        chunkWriter.ScanAllImages();
        Console.WriteLine("All chunks added");
    }

    public void AddAllValidWords()
    {
        Console.WriteLine("Adding all valid words...");
        string[] answerPaths = Directory.GetFiles(paths.QuartilesAnswersFolder);

        foreach (var answerPath in answerPaths)
        {
            updater.AddUpdateFromFile(answerPath);
            Console.WriteLine($"{Path.GetFileName(answerPath)} processed");
        }

        Console.WriteLine("All valid words added");
    }

    public void AllChunkAnswers()
    {
        Console.WriteLine("Updating all dictionaries and invalid words liss...");
        string[] chunkPaths = Directory.GetFiles(paths.ChunkWriterChunkFolder);

        foreach (var chunkPath in chunkPaths)
        {
            string chunkFileName = Path.GetFileName(chunkPath);
            if (!chunkWriter.FileVerified(chunkPath))
            {
                Console.WriteLine($"{chunkFileName} is unverified, skipping");
                continue;
            }

            string datePart = chunkFileName.Substring("quartiles-chunk-".Length, "YYYY-MM-DD".Length);
            string answerFileName = $"quartiles-answers-{datePart}.txt";
            string answerFilePath = Path.Combine(paths.QuartilesAnswersFolder, answerFileName);

            if (!File.Exists(answerFilePath))
            {
                Console.WriteLine($"{answerFileName} does not exist, skipping.");
                continue;
            }

            List<string> chunkList = new List<string>(File.ReadAllLines(chunkPath));
            HashSet<string> answerSet = new HashSet<string>(File.ReadAllLines(answerFilePath));

            var allSolutions = solver.QuartileSolver(chunkList);
            updater.RemoveUpdate(allSolutions, answerSet);

            Console.WriteLine($"Updated {datePart} Quartile");
        }

        Console.WriteLine("Update Complete!");
    }
}