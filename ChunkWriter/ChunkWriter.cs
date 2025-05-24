using System.Text.RegularExpressions;
using Paths;
using QuartilesToText;

/// <summary>
/// Class that scans chunks from Quartile games and writes chunks to files
/// </summary>
class ChunkWriter
{
    private QuartilePaths paths = new QuartilePaths(filesToBeModified: true);

    /// <summary>
    /// Scans all image files in QuartilesToTextImagesFolder and writes the individual chunks to ChunkExtractorChunkFolder
    /// </summary>
    public void ScanAllImages()
    {

        using (var extractor = new QuartilesOCR(filesToBeModified: true))
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
                    string chunkFilePath = Path.Combine(paths.ChunkWriterChunkFolder, chunkFileName);

                    // Only write if a chunk file doesn't exist yet
                    if (!File.Exists(chunkFilePath))
                    {
                        Console.WriteLine($"Writing to {imageFileName}.\n");
                        extractor.ImageName = imageFileName;
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

    }

    /// <summary>
    /// Checks for any files that have not been verified by humans (removed the !!!UNVERIFIED!!! tag at the end of a chunk file)
    /// </summary>
    public void CheckForUnverifiedFiles()
    {
        string[] quartileChunks = Directory.GetFiles(paths.ChunkWriterChunkFolder);
        bool anyUnverifiedFiles = false;

        foreach (string chunkPath in quartileChunks)
        {
            var chunkFileName = Path.GetFileName(chunkPath);
            var chunks = new HashSet<string>(File.ReadAllLines(chunkPath));
            if (chunks.Contains("!!!UNVERIFIED!!!"))
            {
                Console.WriteLine($"{chunkFileName} is unverified!");
                anyUnverifiedFiles = true;
            }
        }

        if(!anyUnverifiedFiles)
        {
            Console.WriteLine("All files are verified.");
        }
    }

    /// <summary>
    /// Writes all chunks to a given chunk file path, appends !!!UNVERIFIED!!! at the end of each file as OCR can be incorrect
    /// </summary>
    /// <param name="chunkFilePath">File path to write chunks to</param>
    /// <param name="chunks">List of chunks to write to file</param>
    private void WriteChunksToFile(string chunkFilePath, List<string> chunks)
    {
        File.WriteAllLines(chunkFilePath, chunks);
        File.AppendAllText(chunkFilePath, "!!!UNVERIFIED!!!"); // Human should verify every file has been correctly scanned and can remove this line after verification
    }

    public static void Main(string[] args)
    {
        var chunkExtractor = new ChunkWriter();
        chunkExtractor.ScanAllImages();
        chunkExtractor.CheckForUnverifiedFiles();
    }
}