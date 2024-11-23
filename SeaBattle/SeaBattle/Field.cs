using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class Field
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public CellState[,] Map;

        public Field(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new CellState[height, width];
        }

        public CellState[,] GenerateField()
        {
            Map = new CellState[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Map[i, j] = CellState.Empty;
                }
            }

            return Map;
        }
    }
}
