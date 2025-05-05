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
        
        dataPath = Path.Combine(QTTRoot, "tessdata");
        imagePath = Path.Combine(imageFolder, imageName);

        if(!File.Exists(imagePath))
        {
            throw new Exception($"Image not found: {imagePath}");
        }

        engine = new TesseractEngine(dataPath, "eng");
        engine.DefaultPageSegMode = PageSegMode.SingleBlock;

        chunks = new List<string>();
    }

    public void ExtractChunks()
    {
        string chunkText;

        using(engine)
        {
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

    public static void Main()
    {
        var extractor = new QTT("quartiles3.png");
        extractor.ExtractChunks();
    }
}