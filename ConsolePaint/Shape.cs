using System;
using System.Collections.Generic;

namespace ConsolePaint
{
    public abstract class Shape
    {
        private static int idCounter = 0;

        // Делаем публичное свойство с сеттером (если нужно восстанавливать из JSON)
        public int Id { get; set; }

        // Чтобы JSON мог инициализировать, делаем свойства вместо полей
        public char Symbol { get; set; }
        public ConsoleColor Color { get; set; }

        // Аналогично для списков
        public List<Pixel> OuterPixels { get; set; }
        public List<Pixel> InnerPixels { get; set; }

        // Публичный конструктор без параметров, нужен для десериализации
        public Shape()
        {
            Id = idCounter++;
            OuterPixels = new List<Pixel>();
            InnerPixels = new List<Pixel>();
            Symbol = ' ';              // какое-то значение по умолчанию
            Color = ConsoleColor.White; // какое-то значение по умолчанию
        }

        // Если нужен ещё и ваш “старый” конструктор — оставляем
        protected Shape(char symbol, ConsoleColor color)
        {
            Id = idCounter++;
            OuterPixels = new List<Pixel>();
            InnerPixels = new List<Pixel>();
            Symbol = symbol;
            Color = color;
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
