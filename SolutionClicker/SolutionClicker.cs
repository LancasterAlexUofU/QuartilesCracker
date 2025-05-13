using System.Runtime.InteropServices;
using Chunks;
using QuartilesToText;
using Quartiles;

/// <summary>
/// Clicks on the screen to solve a quartile game
/// </summary>
public class SolutionClicker
{
    /// <summary>
    /// Extractor that uses OCR to extract the letters from the quartiles game
    /// </summary>
    private QTT extractor;

    /// <summary>
    /// Solver that finds the solutions for the quartile
    /// </summary>
    private QuartilesCracker solver;

    /// <summary>
    /// Name of the image to be scanned for letters, including the file extension
    /// </summary>
    private string imageName;



    /// <summary>
    /// The number of columns in the quartile. 1-Indexed
    /// </summary>
    private int columns;

    /// <summary>
    /// The number of rows in the quartile. 1-Indexed
    /// </summary>
    private int rows;

    /// <summary>
    /// Holds the row index of the last unoccupied row. 0-Indexed
    /// 
    /// <para>
    /// Whenever a max chunk size solution is found, the chunks are moved to the last unoccupied row
    /// </para>
    /// </summary>
    private int lastUnoccupiedRowIndex;

    /// <summary>
    /// Top left corner of the quartile grid. Used for OCR scanning
    /// </summary>
    private Point topLeft;

    /// <summary>
    /// Bottom right corner of the quartile grid. Used for OCR scanning
    /// </summary>
    private Point bottomRight;

    /// <summary>
    /// List of chunks that are used to store the letters and their center positions
    /// </summary>
    private List<Chunk> wordChunks;



    /// <summary>
    /// Current score of the quartile game. Used for checking if the solution is valid
    /// </summary>
    private int score;

    /// <summary>
    /// Top left corner of the score area. Ensure corner contains area for 3-digit numbers. Used for OCR scanning
    /// </summary>
    private Point topLeftScore;

    /// <summary>
    /// Bottom right corner of the score area. Ensure corner contains area for 3-digit numbers. Used for OCR scanning
    /// </summary>
    private Point bottomRightScore;



    /// <summary>
    /// Center position for the check mark button
    /// </summary>
    private Point checkSolution;

    /// <summary>
    /// Center position for the 'x' button to close a popup
    /// </summary>
    private Point closePopup;

    /// <summary>
    /// Boolean to check if the expert level popup has been closed
    /// </summary>
    private bool expertPopupClosed;

    /// <summary>
    /// Boolean to check if the perfect level popup has been closed
    /// </summary>
    private bool perfectPopupClosed;

    public static void Main(string[] args)
    {
        var SolutionClicker = new SolutionClicker("quartiles-2024-06-16.png");
        SolutionClicker.PrintMousePostition();

        SolutionClicker.RunAutoSolver();

        Console.WriteLine($"Final score: {SolutionClicker.score}");
    }

    /// <summary>
    /// Constructor for the SolutionClicker class
    /// </summary>
    /// <param name="imageName">Image filename in QuartilesImages to scan</param>
    public SolutionClicker(string imageName)
    {
        extractor = new QTT(imageName);
        solver = new QuartilesCracker();

        this.imageName = imageName;

        columns = solver.MAX_CHUNKS;
        rows = solver.MAX_LINES;
        lastUnoccupiedRowIndex = rows - 1;
        wordChunks = new List<Chunk>();

        // https://www.quartilesgame.org, 100% Vivaldi
        topLeft = new Point(630, 320);
        bottomRight = new Point(1060, 550);

        score = 0;
        topLeftScore = new Point(635, 200);
        bottomRightScore = new Point(660, 235);

        checkSolution = new Point(1040, 615);
        closePopup = new Point();
        expertPopupClosed = false;
    }

    /// <summary>
    /// Automatically solves the quartile by scanning and clicking on the screen
    /// </summary>
    public void RunAutoSolver()
    {
        SetChunksCenterPosition();
        SetChunksLetters();
        SolveQuartile();
        ClickMaxChunkSize();
        ClickAllChunks();
    }

    /// <summary>
    /// Sets the rows, column, and center position values for each word chunk
    /// 
    /// <para>
    /// Adds chunks from left to right, top to bottom
    /// </para>
    /// </summary>
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

    /// <summary>
    /// Scans the quartile grid and sets the letters in the correct positions
    /// 
    /// <para>
    /// ExtractChunks returns chunks from left to right, top to bottom
    /// </para>
    /// </summary>
    private void SetChunksLetters()
    {
        extractor.ExtractChunks();

        for(int i = 0; i < wordChunks.Count; i++)
        {
            wordChunks[i].Letters = extractor.chunks[i];
        }
    }

    /// <summary>
    /// Finds solutions to quartile game, stored in solver.results
    /// </summary>
    private void SolveQuartile()
    {
        solver.chunks = extractor.chunks;
        solver.QuartilesDriver();
    }

    /// <summary>
    /// Left clicks on a given list of chunks
    /// </summary>
    /// <param name="chunkList"></param>
    private void ClickOnChunks(List<string> chunkList)
    {

        foreach(var chunk in chunkList)
        {
            // Finds the matching chunk in wordChunks
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

    /// <summary>
    /// Clicks the check solution button 
    /// </summary>
    private void ClickCheckSolution()
    {
        Cursor.Position = checkSolution;
        LeftClick();
    }

    /// <summary>
    /// Clicks on the 'x' to close any popup that appears
    /// </summary>
    private void ClickClosePopup()
    {
        Cursor.Position = closePopup;

        Thread.Sleep(1000); // wait for popup to appear
        LeftClick();
    }

    /// <summary>
    /// Call to simulate a left click
    /// </summary>
    private void LeftClick()
    {
        mouse_event(MouseEventFlags.LeftDown, 0, 0, 0, UIntPtr.Zero);
        Thread.Sleep(50); // short delay
        mouse_event(MouseEventFlags.LeftUp, 0, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Takes a screenshot of the current screen to see if the score has increased.
    /// 
    /// <para>
    /// If the score increases, this indicates a solution is valid
    /// </para>
    /// </summary>
    /// <returns>True if the score increased, false otherwise</returns>
    private bool IsSolutionValid()
    {
        int previousScore = score;
        score = GetScore();

        if (score > previousScore)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Takes a screenshot of the current score and returns the value
    /// 
    /// <para>
    /// If the score is empty, it could be due to a popup. The method will attempt to close the popup and try again.
    /// </para>
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Calls GetScore to scan the screen and updates the score variable to reflect the new score.
    /// </summary>
    private void UpdateScore()
    {
        int newScore = GetScore();
        score = newScore;
    }

    /// <summary>
    /// Only clicks on quartile solutions using the max chunk size
    /// 
    /// <para>
    /// Since the chunks only move around for max sized solution, this method is separate
    /// </para>
    /// 
    /// <para>
    /// Once all valid max chunk size solutions have been found, this program will stop
    /// </para>
    /// </summary>
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

    /// <summary>
    /// Clicks on all chunks of max chunk size - 1
    /// </summary>
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

    /// <summary>
    /// Calculates the new position of all chunk after tiles rearrange from a max chunk size solution
    /// </summary>
    /// <param name="validMaxChunks">The chunks that formed a valid solution</param>
    private void CalculateNewValues(List<string> validMaxChunks)
    {
        var sortedChunks = new SortedList<int, Chunk>();

        // Adds chunks to a sorted list ordered by their position value in the 2D grid
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

        // Adds each chunk to the list to find where its position lies to calculate its offset
        // Then, updates the chunk to be at its new position
        foreach (var chunk in wordChunks)
        {
            try
            {
                sortedChunks.Add(chunk.Value, chunk);
                int offset = sortedChunks.IndexOfKey(chunk.Value);
                sortedChunks.Remove(chunk.Value);

                chunk.UpdateRowsAndColumns(chunk.Value - offset);
            }

            // Caught when adding duplicate keys to a sorted list, skips over
            catch (ArgumentException) { }
        }

        // Adds validMaxChunks to the last unoccupied row
        for (int column = 0; column < columns; column++)
        {
            Chunk chunk = sortedChunks.GetValueAtIndex(column);
            chunk.Row = lastUnoccupiedRowIndex;
            chunk.Column = column;
        }

        lastUnoccupiedRowIndex--;
    }

    /// <summary>
    /// Prints the mouse position for debugging purposes
    /// </summary>
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

    [Flags]
    public enum MouseEventFlags : uint
    {
        LeftDown = 0x0002,
        LeftUp = 0x0004,
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern void mouse_event(MouseEventFlags dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
}