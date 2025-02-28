using System;
using System.Collections.Generic;
using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Canvas
    {
        private int width;
        private int height;
        private Pixel[,] pixels;
        private List<Shape> shapes;  // Список фигур на холсте

        public int Height => height;

        public Canvas(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new Pixel[width, height];
            shapes = new List<Shape>();

            DrawFrame();
        }

        /// <summary>
        /// Рисует рамку вокруг внутренней области холста.
        /// Верхняя граница – строка 0, внутренняя область – строки 1..height, нижняя граница – строка (height+1).
        /// </summary>
        public void DrawFrame()
        {
            // Верхняя граница
            Console.SetCursorPosition(0, 0);
            Console.Write(" ");
            for (int i = 0; i < width; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine();

            // Боковые границы
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(0, y + 1);
                Console.Write("|");
                for (int x = 0; x < width; x++)
                {
                    Console.Write(" ");
                }
                Console.Write("|");
            }

            // Нижняя граница
            Console.SetCursorPosition(0, height + 1);
            Console.Write(" ");
            for (int i = 0; i < width; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Отрисовывает один пиксель в пределах внутренней области холста.
        /// Смещаем на (1,1), чтобы не затирать рамку.
        /// </summary>
        public void SetPixel(int x, int y, char symbol, ConsoleColor color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                // Создаём и сохраняем пиксель
                pixels[x, y] = new Pixel(x, y, symbol, color);
                Console.SetCursorPosition(x + 1, y + 1);
                Console.ForegroundColor = color;
                Console.Write(symbol);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Возвращает пиксель по координатам. Если пиксель не создан – возвращает "пустой" пиксель.
        /// </summary>
        public Pixel GetPixel(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return pixels[x, y] ?? new Pixel(x, y, ' ', ConsoleColor.Black);
            }
            return new Pixel(x, y, ' ', ConsoleColor.Black);
        }

        /// <summary>
        /// Очищает внутреннюю область холста (не затрагивая рамку) и сбрасывает массив пикселей.
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixels[x, y] = new Pixel(x, y, ' ', ConsoleColor.Black);
                    Console.SetCursorPosition(x + 1, y + 1);
                    Console.Write(" ");
                }
            }
        }

        /// <summary>
        /// Отрисовывает фигуру: пробегает по спискам OuterPixels и InnerPixels.
        /// </summary>
        public void Draw(Shape shape)
        {
            foreach (var p in shape.OuterPixels)
            {
                SetPixel(p.X, p.Y, p.Symbol, p.Color);
            }
            foreach (var p in shape.InnerPixels)
            {
                SetPixel(p.X, p.Y, p.Symbol, p.Color);
            }
        }

        /// <summary>
        /// Перерисовывает все фигуры из списка shapes.
        /// </summary>
        public void RedrawAllShapes()
        {
            Clear();
            foreach (var s in shapes)
            {
                Draw(s);
            }
        }

        /// <summary>
        /// Добавляет фигуру в список и отрисовывает её.
        /// </summary>
        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
            Draw(shape);
        }

        /// <summary>
        /// Удаляет фигуру из списка и перерисовывает холст.
        /// </summary>
        public void RemoveShape(Shape shape)
        {
            shapes.Remove(shape);
            RedrawAllShapes();
        }

        /// <summary>
        /// Очищает список фигур и внутреннюю область холста.
        /// </summary>
        public void ClearShapes()
        {
            shapes.Clear();
            Clear();
        }

        /// <summary>
        /// Обёртки для создания фигур через статическую фабрику.
        /// Создает линию, добавляет в список и отрисовывает.
        /// </summary>
        public void AddLine(int x1, int y1, int x2, int y2, char symbol = '*', ConsoleColor color = ConsoleColor.White)
        {
            Shape s = ShapeFactory.CreateLine(x1, y1, x2, y2, symbol, color);
            AddShape(s);
        }

        public void AddPoint(int x, int y, char symbol = '*', ConsoleColor color = ConsoleColor.White)
        {
            Shape s = ShapeFactory.CreatePoint(x, y, symbol, color);
            AddShape(s);
        }

        public void AddRectangle(int x1, int y1, int x2, int y2, char symbol = '#', ConsoleColor color = ConsoleColor.White)
        {
            Shape s = ShapeFactory.CreateRectangle(x1, y1, x2, y2, symbol, color);
            AddShape(s);
        }

        /// <summary>
        /// Возвращает список всех фигур.
        /// </summary>
        public List<Shape> GetShapes() => new List<Shape>(shapes);
    }
}
