using System.Text.RegularExpressions;
using Paths;
using QuartilesToText;

class ChunkExtractor
{
    private QuartilePaths paths = new QuartilePaths(filesToBeModified: true);
    public void ScanAllImages()
    {
        // Image files need to be in the form of quartiles-YYYY-MM-DD.png
        string validImageNamePattern = @"quartiles-\d{4}-\d{2}-\d{2}\.png";

        string[] quartileImages = Directory.GetFiles(paths.QuartilesToTextImagesFolder);
        foreach (string image in quartileImages)
        {
            if (Regex.IsMatch(image, validImageNamePattern))
            {
                string imageFileName = Path.GetFileName(image);
                string datePart = imageFileName.Substring("quartiles-".Length, "YYYY-MM-DD".Length);
                string chunkFileName = $"quartiles-chunk-{datePart}.txt";
                string chunkFilePath = Path.Combine(paths.ChunkExtractorChunkFolder, chunkFileName);

                if (!File.Exists(chunkFilePath))
                {
                    Console.WriteLine($"Writing to {imageFileName}.\n");
                    var extractor = new QTT(image);
                    var chunks = extractor.ExtractChunks();
                    WriteChunksToFile(chunkFilePath, chunks);
                }

                else
                {
                    Console.WriteLine($"File {chunkFileName} already exists, skipping\n");
                }
            }
        }
    }

    private void WriteChunksToFile(string chunkFilePath, List<string> chunks)
    {
        File.WriteAllLines(chunkFilePath, chunks);
        File.AppendAllText(chunkFilePath, "!!!UNVERIFIED!!!");
    }

    public static void Main(string[] args)
    {
        var chunkExtractor = new ChunkExtractor();
        chunkExtractor.ScanAllImages();
    }
}