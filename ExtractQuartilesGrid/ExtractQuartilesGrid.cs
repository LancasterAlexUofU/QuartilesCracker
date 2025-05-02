using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Tesseract;
public class QuartilesExtractor
{
    bool debugMode = true; // Set to false for production
    List<string> ExtractQuartilesGrid(string imagePath)
    {
        try
        {
            // First, ensure tessdata directory exists
            string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            if (!Directory.Exists(tessDataPath))
            {
                Console.WriteLine($"Creating tessdata directory at: {tessDataPath}");
                Directory.CreateDirectory(tessDataPath);
                Console.WriteLine($"Please download eng.traineddata and place it in: {tessDataPath}");
                return new List<string>();
            }
            else
            {
                string engDataFile = Path.Combine(tessDataPath, "eng.traineddata");
                if (!File.Exists(engDataFile))
                {
                    Console.WriteLine($"Warning: eng.traineddata not found at {engDataFile}");
                    Console.WriteLine("OCR may not work properly without language data.");
                }
            }

            // Load the image
            Mat src = CvInvoke.Imread(imagePath);
            if (src.IsEmpty)
            {
                Console.WriteLine($"Failed to load image from: {imagePath}");
                return new List<string>();
            }

            if (debugMode)
                Console.WriteLine($"Loaded image of size: {src.Width}x{src.Height}");

            // Convert to grayscale for better processing
            Mat gray = new Mat();
            CvInvoke.CvtColor(src, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            // Apply binary threshold to isolate the white/light gray buttons
            Mat binary = new Mat();
            CvInvoke.Threshold(gray, binary, 200, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

            if (debugMode)
            {
                CvInvoke.Imwrite("debug_gray.jpg", gray);
                CvInvoke.Imwrite("debug_binary.jpg", binary);
            }

            // Find contours
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(binary, contours, hierarchy,
                                  Emgu.CV.CvEnum.RetrType.List,
                                  Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            if (debugMode)
                Console.WriteLine($"Found {contours.Size} contours");

            List<Rectangle> buttonRects = new List<Rectangle>();

            // Filter contours by size and shape to find the buttons
            for (int i = 0; i < contours.Size; i++)
            {
                using (VectorOfPoint contour = contours[i])
                {
                    double area = CvInvoke.ContourArea(contour);

                    // Adjust these values based on your button size
                    if (area > 1000 && area < 10000)
                    {
                        Rectangle rect = CvInvoke.BoundingRectangle(contour);

                        // Check if it's approximately square (button shape)
                        double aspectRatio = (double)rect.Width / rect.Height;
                        if (aspectRatio > 0.8 && aspectRatio < 1.2)
                        {
                            buttonRects.Add(rect);

                            if (debugMode)
                                Console.WriteLine($"Added button: {rect}, Area: {area}, Aspect: {aspectRatio}");
                        }
                    }
                }
            }

            if (buttonRects.Count == 0)
            {
                Console.WriteLine("No buttons detected! Try adjusting the detection parameters.");
                return new List<string>();
            }

            // Sort buttons by row and column (top to bottom, left to right)
            buttonRects = buttonRects
                .OrderBy(r => r.Y / (r.Height * 0.8)) // Group by rows first
                .ThenBy(r => r.X)                    // Then sort by X position
                .ToList();

            // Create a debug image to visualize the detected buttons
            if (debugMode)
            {
                Mat debugImage = src.Clone();
                for (int i = 0; i < buttonRects.Count; i++)
                {
                    CvInvoke.Rectangle(debugImage, buttonRects[i], new MCvScalar(0, 0, 255), 2);
                    CvInvoke.PutText(debugImage, i.ToString(),
                                    new Point(buttonRects[i].X, buttonRects[i].Y),
                                    Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0,
                                    new MCvScalar(255, 0, 0), 2);
                }
                CvInvoke.Imwrite("detected_buttons.jpg", debugImage);
                Console.WriteLine($"Saved debug image with {buttonRects.Count} detected buttons");
            }

            // Initialize Tesseract OCR
            List<string> buttonTexts = new List<string>();

            try
            {
                using (TesseractEngine tesseractEngine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                {
                    for (int i = 0; i < buttonRects.Count; i++)
                    {
                        var rect = buttonRects[i];

                        // Extract the button region
                        Mat buttonRegion = new Mat(src, rect);

                        // Preprocess for OCR - make text more visible
                        Mat processed = new Mat();
                        CvInvoke.CvtColor(buttonRegion, processed, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                        CvInvoke.Threshold(processed, processed, 150, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                        // Upscale for better OCR results
                        Mat upscaled = new Mat();
                        CvInvoke.Resize(processed, upscaled, new Size(processed.Width * 2, processed.Height * 2));

                        if (debugMode)
                            CvInvoke.Imwrite($"button_{i}.jpg", upscaled);

                        // Convert the Mat to Pix format for Tesseract
                        using (Bitmap bitmap = upscaled.ToBitmap())
                        {
                            using (var pix = Pix.LoadFromMemory(BitmapToByteArray(bitmap)))
                            {
                                // Process with Tesseract
                                using (var page = tesseractEngine.Process(pix))
                                {
                                    string text = page.GetText().Trim();
                                    float confidence = page.GetMeanConfidence();

                                    if (debugMode)
                                        Console.WriteLine($"Button {i}: Text='{text}', Confidence={confidence:P}");

                                    buttonTexts.Add(text);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tesseract error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return buttonTexts;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ExtractQuartilesGrid: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return new List<string>();
        }
    }

    // Helper method to convert Bitmap to byte array
    private byte[] BitmapToByteArray(Bitmap bitmap)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Quartiles Extractor");

            // Show the current directory
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"Current directory: {currentDir}");

            // Path to your screenshot image
            string quartilesExtractorRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            string image = "quartiles1.jpg";
            string imagePath = Path.Combine(quartilesExtractorRoot, "QuartilesImages", image);

            Console.WriteLine($"Loading image from: {imagePath}");
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("Image file not found!");
                return;
            }

            // Create an instance of the class
            QuartilesExtractor extractor = new QuartilesExtractor();

            // Call the method with debug mode enabled
            List<string> gridValues = extractor.ExtractQuartilesGrid(imagePath);

            // Print the extracted values
            Console.WriteLine("\nExtracted grid values:");
            for (int i = 0; i < gridValues.Count; i++)
            {
                Console.WriteLine($"{i + 1}: '{gridValues[i]}'");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}


