using QuartilesToText;


QuartilesOCR quartilesOCR = new QuartilesOCR();

quartilesOCR.ImageName = "quartiles-2024-06-26.png";

List<string> chunks = quartilesOCR.ExtractChunks();

quartilesOCR.Dispose();
    
foreach(string chunk in chunks)
{
    Console.WriteLine(chunk);
}