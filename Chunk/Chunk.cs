using System.Drawing;

namespace Chunks
{
    /// <summary>
    /// Class that holds information about word chunks in a quartile
    /// </summary>
    public class Chunk
    {
        // Backing fields
        private int _row;
        private int _column;

        /// <summary>
        /// Letter data contained in a chunk
        /// </summary>
        public string Letters { get; set; }

        /// <summary>
        /// Gets and sets the current row. When set, value is updated
        /// </summary>
        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                UpdateValue();
            }
        }

        /// <summary>
        /// Gets and sets the current row. When set, value is updated
        /// </summary>
        public int Column
        {
            get => _column;
            set
            {
                _column = value;
                UpdateValue();
            }
        }

        /// <summary>
        /// Gets and sets the position of the chunk in a 1D array
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets and sets the center position of the chunk on the screen
        /// </summary>
        public Point CenterPos { get; set; }

        /// <summary>
        /// Gets and sets the largest amount of chunks that can be used to create a solution (aka the column width)
        /// </summary>
        public int MaxChunkSize { get; set; }

        /// <summary>
        /// Constructor for a chunk object
        /// </summary>
        /// <param name="letters">The letter information for a chunk</param>
        /// <param name="row">Row of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="column">Column of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="centerPos">Center position of the chunk on screen</param>
        /// <param name="maxChunkSize">[Optional parameter] Changes the maximum amount of chunks that can be used to form a solution</param>
        public Chunk(string letters, int row, int column, Point? centerPos = null, int maxChunkSize = 4)
        {
            Letters = letters;
            MaxChunkSize = maxChunkSize;
            Row = row;
            Column = column;
            CenterPos = centerPos ?? default;
        }

        /// <summary>
        /// Constructor for a chunk object, with Letters initialized to empty
        /// </summary>
        /// <param name="row">Row of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="column">Column of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="centerPos">Center position of the chunk on screen</param>
        /// <param name="maxChunkSize">The maximum amount of chunks that can be used to form a solution</param>
        public Chunk(int row, int column, Point? centerPos = null, int maxChunkSize = 4)
            : this(string.Empty, row, column, centerPos, maxChunkSize)
        {
        }

        /// <summary>
        /// Given a value (which is a 1D array position), updates the rows and columns
        /// 
        /// <para>
        /// Rows and Columns setters will also update the value
        /// </para>
        /// </summary>
        /// <param name="value">Position of the chunk in a 1D array</param>
        public void UpdateRowsAndColumns(int value)
        {
            Row = value / MaxChunkSize;
            Column = value % MaxChunkSize;
        }

        /// <summary>
        /// Updates the value to be the 1D array position of the new Rows and Columns position
        /// </summary>
        private void UpdateValue()
        {
            Value = Row * MaxChunkSize + Column;
        }
    }
}
