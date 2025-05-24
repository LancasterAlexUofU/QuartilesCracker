using System.Text.RegularExpressions;
using Tesseract;
using Paths;

namespace QuartilesToText;

public class QTT
{
    private QuartilePaths paths = new QuartilePaths(filesToBeModified: false);
    private TesseractEngine engine;

    private int _minChunkSize = 2;
    private int _maxChunkSize = 5;

    /// <summary>
    /// Gets and sets the minimum character size a chunk can be
    /// </summary>
    public int MinChunkSize
    {
        get => _minChunkSize;
        set
        {
            if (value < 1)
            {
                Console.WriteLine("Warning! Minimum chunk size is less than 1. Setting minimum chunk size to 1.");
                _minChunkSize = 1;
            }

            else if (value > MaxChunkSize)
            {
                Console.WriteLine($"Warning! Minimum chunk size is greater than maximum chunk size. Setting minimum chunk size to {MaxChunkSize - 1}");
                _minChunkSize = MaxChunkSize - 1;
            }

            else
            {
                _minChunkSize = value;
            }
        }
    }

    /// <summary>
    /// Gets and sets the maximum character size a chunk can be
    /// </summary>
    public int MaxChunkSize
    {
        get => _maxChunkSize;
        set
        {
            if (value < MinChunkSize)
            {
                Console.WriteLine($"Warning! Maximum chunks size is smaller than minimum chunk size. Setting maximum chunk size to {MinChunkSize + 1}");
                _maxChunkSize = MinChunkSize + 1;
            }

            else
            {
                _maxChunkSize = value;
            }
        }
    }

    public string ImageName { get; set; }

    public string ImagePath { get => Path.Combine(paths.QuartilesToTextImagesFolder, ImageName); set { } }

    public QTT(string imageName)
    {
        ImageName = imageName;

        // LSTM engine is a neural network engine and is usually better than the legacy engine
        // Using data from tessdata_best/eng.traineddata (which is specifically for LSTM)
        // If you change the engine mode back to using legacy, the eng.traineddata will also need to be changed to the correct one
        engine = new TesseractEngine(paths.QuartilesToTextTessdataFolder, "eng", EngineMode.LstmOnly);
        engine.DefaultPageSegMode = PageSegMode.SingleBlock;
    }

    public List<string> ExtractChunks()
    {
        string chunkText;
        List<string> chunks = [];

        // Only matches lower case letters between minimum and maximum sizes separated by non-word characters
        string chunkPattern = $@"\b[a-z]{{{MinChunkSize},{MaxChunkSize}}}\b";

        Pix image = Pix.LoadFromFile(ImagePath);
        Page page = engine.Process(image);
        chunkText = page.GetText().ToLower();

        var matches = Regex.Matches(chunkText, chunkPattern);

        foreach(Match match in matches)
        {
            chunks.Add(match.Value);
        }

        return chunks;
    }

    public string ExtractScore()
    {
        string score;

        Pix image = Pix.LoadFromFile(ImagePath);

        // Using statement so multiple engines don't try to process the same image
        using (Page page = engine.Process(image))
        {
            score = page.GetText();
        }

        return score;
    }
}