using System.Text.RegularExpressions;
using Tesseract;

namespace QuartilesToText;

public class QTT
{
    private const int MAX_CHUNK_SIZE = 5;

    // Only matches lower case letters between size 1-max size separated by non-word characters
    private string regex = $@"\b[a-z]{{1,{MAX_CHUNK_SIZE}}}\b";

    private string dataPath;
    public string imageFolder;
    public string chunkFolder;
    private string imagePath;
    private TesseractEngine engine;

    public List<string> chunks;

    public QTT(string imageName)
    {
        // Go back 3 levels from bin\debug\netX.Y to get the project directory
        string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
        string QTTRoot = Path.GetFullPath(Path.Combine(projectRoot, @"..\QuartilesToText"));
        imageFolder = Path.Combine(QTTRoot, "QuartileImages");
        chunkFolder = Path.Combine(QTTRoot, "QuartileChunks");
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

        Pix image = Pix.LoadFromFile(imagePath);
        Page page = engine.Process(image);
        chunkText = page.GetText().ToLower();


        var matches = Regex.Matches(chunkText, regex);

        foreach(Match match in matches)
        {
            chunks.Add(match.Value);
        }
    }

    public string ExtractScore()
    {
        string score;

        Pix image = Pix.LoadFromFile(imagePath);

        // Seperate using statement for page so multiple engines don't try to process the same image
        using (Page page = engine.Process(image))
        {
            score = page.GetText();
        }

        return score;
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

    private void WriteChunksToFile(string chunkFilePath)
    {
        File.WriteAllLines(chunkFilePath, chunks);
        File.AppendAllText(chunkFilePath, "!!!UNVERIFIED!!!");
    }

    public void ScanAllImages()
    {
        // Image files need to be in the form of quartiles-YYYY-MM-DD.png
        string validImageNamePattern = @"quartiles-\d{4}-\d{2}-\d{2}\.png";

        string[] quartileImages = Directory.GetFiles(imageFolder);
        foreach (string image in quartileImages)
        {
            if(Regex.IsMatch(image, validImageNamePattern))
            {
                string imageFileName = Path.GetFileName(image);
                string datePart = imageFileName.Substring("quartiles-".Length, "YYYY-MM-DD".Length);
                string chunkFileName = $"quartiles-chunk-{datePart}.txt";
                string chunkFilePath = Path.Combine(chunkFolder, chunkFileName);

                if(!File.Exists(chunkFilePath))
                {
                    Console.WriteLine($"Writing to {imageFileName}.\n");
                    var extractor = new QTT(image);
                    extractor.ExtractChunks();
                    extractor.WriteChunksToFile(chunkFilePath);
                }

                else
                {
                    Console.WriteLine($"File {chunkFileName} already exists, skipping\n");
                }
            }
        }
    }

    public static void Main()
    {
        //string date = "2024-11-06";

        //string imageName = $"quartiles-{date}.png";

        string imageName = "quartiles-2024-06-16.png";

        var extractor = new QTT(imageName);

        extractor.ScanAllImages();
        //extractor.ExtractChunks();
        //extractor.PrintChunks();
    }
}