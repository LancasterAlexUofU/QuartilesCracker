using Tesseract;

// Go back 3 levels from bin\debug\netX.Y to save the file merge in the project folder, not the debug folder
string QTTRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));

string dataPath = Path.Combine(QTTRoot, "tessdata");
string imageFolder = Path.Combine(QTTRoot, "QuartileImages");

string imageName = "quartiles1.png";
string imagePath = Path.Combine(imageFolder, imageName);

string outputPath = Path.Combine(QTTRoot, "chunks.txt");

using (var engine = new TesseractEngine(dataPath, "eng"))
{
    engine.DefaultPageSegMode = PageSegMode.SingleBlock; // SingleBlock works best for grid images

    var image = Pix.LoadFromFile(imagePath);
    var page = engine.Process(image);
    var text = page.GetText();

    File.WriteAllText(outputPath, text);
}