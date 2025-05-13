using System.Drawing;

namespace Chunks
{
    /// <summary>
    /// Class that holds information about word chunks in a quartile
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// Letter data contained in a chunk
        /// </summary>
        public string Letters { get; set; }

        /// <summary>
        /// Helper variable for Row getter and setter
        /// </summary>
        private int row;

        /// <summary>
        /// Gets and sets the current row. When set, value is updated
        /// </summary>
        public int Row
        {
            get => row;
            set
            {
                row = value;
                UpdateValue();
            }
        }

        /// <summary>
        /// Helper variable for Column getter and setter
        /// </summary>
        private int column;

        /// <summary>
        /// Gets and set the current row. When set, value is updated
        /// </summary>
        public int Column
        {
            get => column;
            set
            {
                column = value;
                UpdateValue();
            }
        }

        /// <summary>
        /// Position of the chunk in a 1D array
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Center position of the chunk on the screen
        /// </summary>
        public Point CenterPos { get; set; }

        /// <summary>
        /// Largest amount of chunks that can be used to create a solution (aka the column width)
        /// </summary>
        private int maxChunkSize;

        /// <summary>
        /// Constructor for a chunk object
        /// </summary>
        /// <param name="letters">The letter information for a chunk</param>
        /// <param name="row">Row of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="column">Column of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="centerPos">Center position of the chunk on screen</param>
        /// <param name="maxChunkSize">[Optional parameter] Changes the maximum amount of chunks that can be used to form a solution</param>
        public Chunk(string letters, int row, int column, Point centerPos, int maxChunkSize = 4)
        {
            Letters = letters;
            Row = row;
            Column = column;
            CenterPos = centerPos;
            this.maxChunkSize = maxChunkSize;
        }

        /// <summary>
        /// Constructor for a chunk object, with Letters initialized to empty
        /// </summary>
        /// <param name="row">Row of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="column">Column of the chunk in the quartiles grid, 0-indexed</param>
        /// <param name="centerPos">Center position of the chunk on screen</param>
        /// <param name="maxChunkSize">[Optional parameter] Changes the maximum amount of chunks that can be used to form a solution</param>
        public Chunk(int row, int column, Point centerPos, int maxChunkSize = 4)
        {
            Letters = string.Empty;
            this.maxChunkSize = maxChunkSize;
            Row = row;
            Column = column;
            CenterPos = centerPos;
        }

        /// <summary>
        /// Given a value (which is a 1D array position), updates the rows and columns
        /// 
        /// <para>
        /// Rows and Columns setters will also update the value
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        public void UpdateRowsAndColumns(int value)
        {
            Row = value / maxChunkSize;
            Column = value % maxChunkSize;
        }

        /// <summary>
        /// Updates the value to be the 1D array position of the new Rows and Columns position
        /// </summary>
        private void UpdateValue()
        {
            Value = Row * maxChunkSize + Column;
        }

    }
}
