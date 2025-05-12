using Chunks;
using QuartilesToText;
using Quartiles;
using System.Runtime.InteropServices;

public class SolutionClicker
{
    private QTT extractor;
    private QuartilesCracker solver;

    private string imageName;

    private int columns;
    private int rows;
    private int lastUnoccupiedRowIndex;

    Point topLeft;
    Point bottomRight;

    private int score;
    Point topLeftScore;
    Point bottomRightScore;

    private Point checkSolution;
    private Point closePopup;
    private bool expertPopupClosed;
    private bool perfectPopupClosed;

    private List<Chunk> wordChunks;


    public static void Main(string[] args)
    {
        var SolutionClicker = new SolutionClicker("quartiles-2024-06-16.png");
        SolutionClicker.PrintMousePostition();
    }

    public SolutionClicker(string imageName)
    {
        extractor = new QTT(imageName);
        solver = new QuartilesCracker();

        this.imageName = imageName;

        columns = solver.MAX_CHUNKS;
        rows = solver.MAX_LINES;
        lastUnoccupiedRowIndex = rows - 1;

        // https://www.quartilesgame.org, 100% Vivaldi
        topLeft = new Point(630, 320);
        bottomRight = new Point(1060, 550);

        score = 0;
        topLeftScore = new Point(635, 200);
        bottomRightScore = new Point(660, 235);

        checkSolution = new Point(1040, 615);
        closePopup = new Point();
        expertPopupClosed = false;
        

        wordChunks = new List<Chunk>();


        SetChunksCenterPosition();
        SetChunksLetters();
        SolveQuartile();
        ClickMaxChunkSize();
        ClickAllChunks();

        Console.WriteLine($"Final score: {score}");
    }

    private void SetChunksCenterPosition()
    {
        int cellWidth = (bottomRight.X - topLeft.X) / columns;
        int cellHeight = (bottomRight.Y - topLeft.Y) / rows;

        // Center of a cell for a given row and column
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int centerX = topLeft.X + cellWidth * col + cellWidth / 2;
                int centerY = topLeft.Y + cellHeight * row + cellHeight / 2;
                Point cellCenter = new Point(centerX, centerY);

                var chunk = new Chunk(row, col, cellCenter);
                wordChunks.Add(chunk);
            }
        }
    }

    private void SetChunksLetters()
    {
        extractor.ExtractChunks();

        for(int i = 0; i < wordChunks.Count; i++)
        {
            wordChunks[i].Letters = extractor.chunks[i];
        }
    }

    private void SolveQuartile()
    {
        solver.chunks = extractor.chunks;
        solver.QuartilesDriver();
    }

    private void ClickOnChunks(List<string> chunkList)
    {
        // For each key in wordChunkMapping, extract the list of chunks
        // For each chunk, find the equivalent letters in wordChunks
        // Then, add the center position of that chunk to solutionClicks

        foreach(var chunk in chunkList)
        {
            Chunk matchedChunk = wordChunks.FirstOrDefault(c => c.Letters == chunk);

            if (matchedChunk != null)
            {
                Cursor.Position = matchedChunk.CenterPos;
                LeftClick();
            }

            else
            {
                Console.WriteLine($"Could not find chunk with letters: {chunk}");
            }
        }
    }
    private void ClickCheckSolution()
    {
        Cursor.Position = checkSolution;
        LeftClick();
    }

    private void ClickClosePopup()
    {
        Cursor.Position = closePopup;

        Thread.Sleep(1000); // wait for popup to appear
        LeftClick();
    }

    private void LeftClick()
    {
        mouse_event(MouseEventFlags.LeftDown, 0, 0, 0, UIntPtr.Zero);
        Thread.Sleep(50); // short delay
        mouse_event(MouseEventFlags.LeftUp, 0, 0, 0, UIntPtr.Zero);
    }

    private bool IsSolutionValid()
    {
        // Checking using OCR if the score increased. If so, the solution is valid
        // Really only need to check for max chunk size solutions
        // For now, just returning true

        int previousScore = score;
        score = GetScore();

        if (score > previousScore)
        {
            return true;
        }

        return false;
    }

    private int GetScore()
    {
        // Takes a screenshot of the score area and uses OCR to extract the score
        // For now, just returning 0

        // Wait for animation to complete
        Thread.Sleep(500);

        string scannedScore = string.Empty;
        int scannedScoreInt;

        //int scannedScoreInt = 

        // Likely that the expert level popup is blocking the score (highest score word is 8 points, so withing feasible range of triggering popup)
        if (scannedScore.Equals(string.Empty) && (score >= 92 && score <= 99) && !expertPopupClosed)
        {
            Console.WriteLine("No score found. Assuming expert level popup is blocking. Closing expert level popup.");
            ClickClosePopup();
            expertPopupClosed = true;

            // Try again after likely closing the popup
            return GetScore();
        }

        else if(scannedScore.Equals(string.Empty) && score >= 100 && !perfectPopupClosed)
        {
            Console.WriteLine("No score found. Assuming perfect level popup is blocking. Closing perfect level popup.");
            ClickClosePopup();
            perfectPopupClosed = true;

            // Try again after likely closing the popup
            return GetScore();
        }

        // Possible that a popup appeared, but could indicate a read error since score is not in range
        else if(scannedScore.Equals(string.Empty) && (!expertPopupClosed || !perfectPopupClosed))
        {
            Console.WriteLine("No score found. Possible that a popup is blocking or error reading.");
            ClickClosePopup();

            // Assume either popup was closed
            expertPopupClosed = true;
            perfectPopupClosed = true;

            // Try again
            return GetScore();
        }

        else
        {
            if (int.TryParse(scannedScore, out scannedScoreInt))
            {
                Console.WriteLine($"New score: {scannedScoreInt}");
                return scannedScoreInt;
            }

            else
            {
                Console.WriteLine("Invalid or empty score scan. Returning last known score.");
                return score;
            }
        }
    }

    private void UpdateScore()
    {
        int newScore = GetScore();
        score = newScore;
    }


    private void ClickMaxChunkSize()
    {
        // Keeps track of how many solutions have been solved using the maximum amount of chunks
        int maxSolutionsSolved = 0;

        foreach (var kvp in solver.wordChunkMapping)
        {
            List<string> chunks = kvp.Value;

            if (chunks.Count == solver.MAX_CHUNKS)
            {
                ClickOnChunks(chunks);
                ClickCheckSolution();

                if (IsSolutionValid())
                {
                    maxSolutionsSolved++;
                    CalculateNewValues(chunks);
                }

                // Stop after finding all solutions for max chunk size
                if (maxSolutionsSolved >= rows)
                {
                    break;
                }
            }

            else if (chunks.Count < solver.MAX_CHUNKS)
            {
                Console.WriteLine($"Warning! Could not find all solutions for chunk size {solver.MAX_CHUNKS}. Found {maxSolutionsSolved} solutions.");
                break;
            }
        }
    }

    private void ClickAllChunks()
    {
        int startIndex = GetFirstNonMaxChunkSizeIndex();

        for (int i = startIndex; i < solver.wordChunkMapping.Count; i++)
        {
            var kvp = solver.wordChunkMapping[i];
            List<string> chunks = kvp.Value;
            ClickOnChunks(chunks);
            ClickCheckSolution();
            UpdateScore();
        }
    }

    /// <summary>
    /// Skips through all max chunk size solutions until it reaches the first solution that is not max chunk size
    /// <para>
    /// Assumes that max chunk size solutions are added first
    /// </para> 
    /// </summary>
    /// <returns>The first index of a solution that uses less than max chunk size</returns>
    private int GetFirstNonMaxChunkSizeIndex()
    {
        foreach (var kvp in solver.wordChunkMapping)
        {
            List<string> chunks = kvp.Value;

            if (chunks.Count < solver.MAX_CHUNKS)
            {
                return solver.wordChunkMapping.IndexOf(kvp);
            }
        }

        Console.WriteLine($"Warning! Could not find any solutions less than chunk size {solver.MAX_CHUNKS}.");
        return solver.wordChunkMapping.Count - 1;
    }

    private void CalculateNewValues(List<string> validMaxChunks)
    {
        var sortedChunks = new SortedList<int, Chunk>();

        foreach (var chunk in validMaxChunks)
        {
            Chunk matchedChunk = wordChunks.FirstOrDefault(c => c.Letters == chunk);

            if (matchedChunk != null)
            {
                sortedChunks.Add(matchedChunk.Value, matchedChunk);
            }

            else
            {
                Console.WriteLine($"Could not find chunk with letters: {chunk}");
            }
        }

        // Calculates and updates the value offsets for each chunk not in the sorted list
        foreach (var chunk in wordChunks)
        {
            try
            {
                sortedChunks.Add(chunk.Value, chunk);
                int offset = sortedChunks.IndexOfKey(chunk.Value);
                sortedChunks.Remove(chunk.Value);

                chunk.UpdateRowsAndColumns(chunk.Value - offset);
            }

            catch (ArgumentException) { }
        }

        for (int column = 0; column < columns; column++)
        {
            Chunk chunk = sortedChunks.GetValueAtIndex(column);
            chunk.Row = lastUnoccupiedRowIndex;
            chunk.Column = column;
        }

        lastUnoccupiedRowIndex--;
    }

    [Flags]
    public enum MouseEventFlags : uint
    {
        LeftDown = 0x0002,
        LeftUp = 0x0004,
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern void mouse_event(MouseEventFlags dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    private void PrintMousePostition()
    {
        Console.WriteLine("Press ESC to stop.");
        while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
        {
            Point pos = Cursor.Position;
            Console.WriteLine($"X: {pos.X}, Y: {pos.Y}");
            Thread.Sleep(500); // Update every half second
        }
    }
}