using System;
using System.Collections.Generic;

namespace ConsolePaint
{
    public abstract class Shape
    {
        public int Id { get; }  // Уникальный идентификатор для каждой фигуры
        private static int idCounter = 0;   // Счётчик для генерации ID
        public List<Pixel> OuterPixels { get; private set; }
        public List<Pixel> InnerPixels { get; private set; }

        private protected char symbol;
        private protected ConsoleColor color;

        public Shape(char symbol, ConsoleColor color)
        {
            Id = idCounter++;
            OuterPixels = [];  
            InnerPixels = [];  
            this.symbol = symbol;
            this.color = color;
        }

        // Метод для вычисления пикселей фигуры 
        protected abstract void CalculatePixels();

        /// <summary>
        /// Перемещает фигуру на (dx, dy): для каждого пикселя в списках прибавляет dx и dy,
        /// затем вызывает CalculatePixels() для пересчёта.
        /// </summary>
        public virtual void Move(int dx, int dy)
        {
            for (int i = 0; i < OuterPixels.Count; i++)
            {
                OuterPixels[i].X += dx;
                OuterPixels[i].Y += dy;
            }
            for (int i = 0; i < InnerPixels.Count; i++)
            {
                InnerPixels[i].X += dx;
                InnerPixels[i].Y += dy;
            }
            // CalculatePixels();
        }

        /// <summary>
        /// Проверяет, содержит ли фигура точку (x, y) в своих пикселях.
        /// </summary>
        public virtual bool ContainsPoint(int x, int y)
        {
            foreach (Pixel p in OuterPixels)
            {
                if (p.X == x && p.Y == y)
                    return true;
            }
            foreach (Pixel p in InnerPixels)
            {
                if (p.X == x && p.Y == y)
                    return true;
            }
            return false;
        }
    }
}
