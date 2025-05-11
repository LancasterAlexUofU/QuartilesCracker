using System.Text.RegularExpressions;
using Tesseract;

namespace QuartilesToText;

public class QTT
{
    private const int MAX_CHUNK_SIZE = 5;

    // Only matches lower case letters between size 1-max size separated by non-word characters
    private string regex = $@"\b[a-z]{{1,{MAX_CHUNK_SIZE}}}\b";

    private string dataPath;
    private string imagePath;
    private TesseractEngine engine;

    public List<string> chunks;

    public QTT(string imageName)
    {
        // Go back 3 levels from bin\debug\netX.Y to get the project directory
        string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
        string QTTRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesToText"));
        string imageFolder = Path.Combine(QTTRoot, "QuartileImages");
        imagePath = Path.Combine(imageFolder, imageName);
        dataPath = Path.Combine(QTTRoot, "tessdata");
        chunks = new List<string>();

        if (!File.Exists(imagePath))
        {
            throw new Exception($"Image not found: {imagePath}");
        }

        if(!Directory.Exists(dataPath))
        {
            throw new Exception($"Tesseract data path not found: {dataPath}");
        }

        // LSTM engine is a neural network engine and is usually better than the legacy engine
        // Using data from tessdata_best/eng.traineddata (which is specifically for LSTM)
        // If you change the engine mode back to using legacy, the eng.traineddata will also need to be changed to the correct one
        engine = new TesseractEngine(dataPath, "eng", EngineMode.LstmOnly);
        engine.DefaultPageSegMode = PageSegMode.SingleBlock;
    }

    public void ExtractChunks()
    {
        string chunkText;

        using(engine)
        {
            //Pix image = LoadHighResImage(imagePath);
            Pix image = Pix.LoadFromFile(imagePath);
            Page page = engine.Process(image);
            chunkText = page.GetText().ToLower();
        }

        var matches = Regex.Matches(chunkText, regex);

        foreach(Match match in matches)
        {
            chunks.Add(match.Value);
        }
    }

    public void PrintChunks()
    {
        if(chunks.Count != 20)
        {
            Console.WriteLine($"Warning!! Expected 20 chunks, but only contained {chunks.Count} chunks.");
        }

        foreach (string chunk in chunks)
        {
            Console.WriteLine(chunk);
        }
    }

    public static void Main()
    {
        string date = "2024-11-06";

        string imageName = $"quartiles-{date}.png";

        var extractor = new QTT(imageName);
        extractor.ExtractChunks();
        extractor.PrintChunks();
    }
}