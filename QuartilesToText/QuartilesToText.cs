using System.Text.RegularExpressions;
using Tesseract;
using Paths;

namespace QuartilesToText;

/// <summary>
/// Library that extract text from quartile games images
/// </summary>
public class QuartilesOCR : IDisposable
{
    private QuartilePaths paths;
    private TesseractEngine engine;
    private bool disposed = false;

    private int _minChunkSize = 2;
    private int _maxChunkSize = 5;

    private string _imageName;

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

    /// <summary>
    /// Gets and sets the file name of the image to scan. File name MUST include the file extension
    /// </summary>
    public string ImageName
    {
        get => _imageName;
        set
        {
            if(!Path.HasExtension(value))
            {
                throw new Exception($"Image name {value} has no file extension.");
            }

            _imageName = value;
        }
    }

    /// <summary>
    /// Gets and sets the path of the image to scan.
    /// </summary>
    public string ImagePath
    {
        // Better practice to set ImageName and get ImagePath rather than set and get ImagePath
        get => Path.Combine(paths.QuartilesToTextImagesFolder, ImageName);
        set
        {
            paths.VerifyFile(value);

            try
            {
                paths.VerifyFileName(value, ImageName);
            }

            catch
            {
                Console.WriteLine("Warning! Image path has a different file name than image name. Setting image name equal to image path's file name.");
                ImageName = Path.GetFileName(value);
            }
        }
    }

    /// <summary>
    /// Default constructor for QuartilesOCR. Important: need to set ImageName before running extractor
    /// </summary>
    /// <param name="filesToBeModified">If files are modified, set this to true</param>
    public QuartilesOCR(bool filesToBeModified = false) : this(imageName: ".png", filesToBeModified) { }

    /// <summary>
    /// Constructor that sets an image name for scanning
    /// </summary>
    /// <param name="imageName">File name of the image to be scanned, including file extension</param>
    /// <param name="filesToBeModified">If files are modified, set this to true</param>
    public QuartilesOCR(string imageName, bool filesToBeModified = false)
    {
        ImageName = imageName;
        paths = new QuartilePaths(filesToBeModified);

        // LSTM engine is a neural network engine and is usually better than the legacy engine
        // Using data from tessdata_best/eng.traineddata (which is specifically for LSTM)
        // If you change the engine mode back to using legacy, the eng.traineddata will also need to be changed to the correct one
        engine = new TesseractEngine(paths.QuartilesToTextTessdataFolder, "eng", EngineMode.LstmOnly);
        engine.DefaultPageSegMode = PageSegMode.SingleBlock;
    }

    /// <summary>
    /// Extracts chunks from a Quartiles game image. Note: Scans be inaccurate, especially with 'i's and 'l's. Always verify
    /// </summary>
    /// <returns>A list of chunks found in the image</returns>
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

    /// <summary>
    /// Given an image of a score number, extracts the score
    /// </summary>
    /// <returns>A string of the scanned score</returns>
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

    // Engine Disposal - Use a using statement for an extractor for proper disposal
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                engine?.Dispose();
            }

            disposed = true;
        }
    }
    ~QuartilesOCR()
    {
        Dispose(false);
    }
}