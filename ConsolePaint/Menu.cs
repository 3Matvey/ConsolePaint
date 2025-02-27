using System;
using System.Collections.Generic;
using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Menu
    {
        private Canvas canvas;
        private List<Shape> shapes;

        public Menu(Canvas canvas)
        {
            this.canvas = canvas;
            shapes = new List<Shape>();
        }

        // Выводит меню и обрабатывает команды.
        public void ShowMenu()
        {
            Console.Clear();
            DrawMenuHeader();
            Console.WriteLine("Выберите команду:");
            Console.WriteLine("1. Нарисовать фигуру (draw)");
            Console.WriteLine("2. Очистить холст (clear)");
            Console.WriteLine("3. Выйти (exit)");
            string choice = Console.ReadLine().ToLower();

            switch (choice)
            {
                case "1":
                    HandleDrawCommand();
                    break;
                case "2":
                    canvas.Clear();
                    shapes.Clear();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    break;
            }
        }

        // Обрабатывает команду создания фигуры.
        private void HandleDrawCommand()
        {
            Console.Clear();
            DrawMenuHeader();
            Console.WriteLine("Выберите тип фигуры:");
            Console.WriteLine("1. Линия");
            Console.WriteLine("2. Точка");
            Console.WriteLine("3. Прямоугольник");
            string choice = Console.ReadLine().ToLower();
            Shape shape = null;

            switch (choice)
            {
                case "1":
                    shape = CreateLine();
                    break;
                case "2":
                    shape = CreatePoint();
                    break;
                case "3":
                    shape = CreateRectangle();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    return;
            }

            shapes.Add(shape);
            canvas.Draw(shape);
            Console.WriteLine("Фигура создана. Нажмите Enter для продолжения.");
            Console.ReadLine();
        }

        private Shape CreateLine()
        {
            Console.WriteLine("Введите координаты начала линии (x1 y1):");
            string[] parts = Console.ReadLine().Split();
            int x1 = int.Parse(parts[0]);
            int y1 = int.Parse(parts[1]);

            Console.WriteLine("Введите координаты конца линии (x2 y2):");
            parts = Console.ReadLine().Split();
            int x2 = int.Parse(parts[0]);
            int y2 = int.Parse(parts[1]);

            Console.WriteLine("Введите символ для линии (по умолчанию '*'):");
            string input = Console.ReadLine();
            char symbol = string.IsNullOrEmpty(input) ? '*' : input[0];

            Console.WriteLine("Введите цвет для линии (например, Red) (по умолчанию White):");
            input = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(input, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateLine(x1, y1, x2, y2, symbol, color);
        }

        private Shape CreatePoint()
        {
            Console.WriteLine("Введите координаты точки (x y):");
            string[] parts = Console.ReadLine().Split();
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            Console.WriteLine("Введите символ для точки (по умолчанию '*'):");
            string input = Console.ReadLine();
            char symbol = string.IsNullOrEmpty(input) ? '*' : input[0];

            Console.WriteLine("Введите цвет для точки (например, Green) (по умолчанию White):");
            input = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(input, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreatePoint(x, y, symbol, color);
        }

        private Shape CreateRectangle()
        {
            Console.WriteLine("Введите координаты верхнего левого угла (x1 y1):");
            string[] parts = Console.ReadLine().Split();
            int x1 = int.Parse(parts[0]);
            int y1 = int.Parse(parts[1]);

            Console.WriteLine("Введите координаты нижнего правого угла (x2 y2):");
            parts = Console.ReadLine().Split();
            int x2 = int.Parse(parts[0]);
            int y2 = int.Parse(parts[1]);

            Console.WriteLine("Введите символ для прямоугольника (по умолчанию '#'):");
            string input = Console.ReadLine();
            char symbol = string.IsNullOrEmpty(input) ? '#' : input[0];

            Console.WriteLine("Введите цвет для прямоугольника (например, Blue) (по умолчанию White):");
            input = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(input, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateRectangle(x1, y1, x2, y2, symbol, color);
        }

        private void DrawMenuHeader()
        {
            Console.WriteLine("=== Меню фигур ===");
        }

        // Позволяет получить список созданных фигур для отрисовки в Terminal.
        public List<Shape> GetShapes()
        {
            return shapes;
        }
    }
}
