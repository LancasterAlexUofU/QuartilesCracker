using System.Drawing;

namespace Chunks
{
    public class Chunk
    {
        public string Letters { get; set; }

        private int row;
        public int Row
        {
            get => row;
            set
            {
                row = value;
                UpdateValue();
            }
        }

        private int column;
        public int Column
        {
            get => column;
            set
            {
                column = value;
                UpdateValue();
            }
        }

        public int Value { get; private set; }
        public Point CenterPos { get; set; }

        private int maxChunkSize;

        public Chunk(string letters, int row, int column, Point centerPos, int maxChunkSize = 4)
        {
            Letters = letters;
            Row = row;
            Column = column;
            CenterPos = centerPos;
            this.maxChunkSize = maxChunkSize;
        }

        public Chunk(int row, int column, Point centerPos, int maxChunkSize = 4)
        {
            Letters = string.Empty;
            Row = row;
            Column = column;
            CenterPos = centerPos;
            this.maxChunkSize = maxChunkSize;
        }

        public void UpdateRowsAndColumns(int value)
        {
            Row = value / maxChunkSize;
            Column = value % maxChunkSize;
        }
        private void UpdateValue()
        {
            Value = Row * maxChunkSize + Column;
        }

    }
}
