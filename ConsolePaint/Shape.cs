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

        public Shape()
        {
            Id = idCounter++;
            OuterPixels = [];  // Инициализация списка
            InnerPixels = [];  // Инициализация списка
        }

        // Метод для вычисления пикселей фигуры (сделаем его публичным)
        public abstract void CalculatePixels();
    }
}
