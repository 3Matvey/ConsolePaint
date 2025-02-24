using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePaint
{
    class Canvas(int width, int height)
    {
        private readonly char[,] grid = new char[height, width];

        public void Clear()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = ' ';
                }
            }
        }

        public void Display()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(grid[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void Draw(Shape shape)
        {
            shape.Draw(this);
        }

        public void SetPixel(int x, int y, char symbol)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                grid[y, x] = symbol;
            }
        }
    }
}