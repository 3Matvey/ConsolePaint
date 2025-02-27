using System;
using System.Collections.Generic;
using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Terminal
    {
        private Canvas canvas;
        private Menu menu;
        private int cursorX;
        private int cursorY;
        private int canvasWidth;
        private int canvasHeight;

        public Terminal(int canvasWidth, int canvasHeight)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            canvas = new Canvas(canvasWidth, canvasHeight);
            menu = new Menu(canvas);
            // Инициализируем курсор в левом верхнем углу холста
            cursorX = 0;
            cursorY = 0;
        }

        public void Run()
        {
            Console.Clear();
            // Первичная отрисовка: рамка холста и меню
            canvas.DrawFrame();
            DrawMenu();
            DrawCursor();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                bool updateDisplay = false;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursorY > 0) { cursorY--; updateDisplay = true; }
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursorY < canvasHeight - 1) { cursorY++; updateDisplay = true; }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursorX > 0) { cursorX--; updateDisplay = true; }
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursorX < canvasWidth - 1) { cursorX++; updateDisplay = true; }
                        break;
                    case ConsoleKey.D:
                        // Вызываем меню для создания новой фигуры
                        menu.ShowMenu();
                        updateDisplay = true;
                        break;
                    case ConsoleKey.Escape:
                        return; // Выход из приложения
                }

                if (updateDisplay)
                {
                    // Перерисовываем рамку и меню, затем курсор
                    canvas.DrawFrame();
                    DrawMenu();
                    DrawCursor();
                }
            }
        }

        // Метод для отрисовки меню под холстом
        private void DrawMenu()
        {
            // Предположим, что рамка холста занимает canvasHeight + 5 строк
            int menuRow = canvasHeight + 6;
            Console.SetCursorPosition(0, menuRow);
            Console.WriteLine("Меню: [D] - Нарисовать фигуру, [Esc] - Выход. Используйте стрелки для перемещения курсора.");
        }

        // Метод для отрисовки курсора на холсте
        private void DrawCursor()
        {
            // Сохраняем текущую позицию курсора
            int prevLeft = Console.CursorLeft;
            int prevTop = Console.CursorTop;

            // Вычисляем позицию на экране с учётом смещения рамки (рамка начинается с позиции (1,5))
            int drawX = cursorX + 1;
            int drawY = cursorY + 5;

            Console.SetCursorPosition(drawX, drawY);
            Console.ForegroundColor = ConsoleColor.Yellow; // Цвет курсора
            Console.Write("_");  // Символ курсора
            Console.ForegroundColor = ConsoleColor.White;

            // Восстанавливаем предыдущую позицию курсора (опционально)
            Console.SetCursorPosition(prevLeft, prevTop);
        }
    }
}
