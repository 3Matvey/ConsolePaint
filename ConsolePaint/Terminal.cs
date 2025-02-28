using System;
using System.Collections.Generic;
using ConsolePaint;
using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Terminal
    {
        private Canvas canvas;

        // Размер внутренней области холста
        private int canvasWidth;
        private int canvasHeight;

        // Положение курсора (0..canvasWidth-1, 0..canvasHeight-1)
        private int cursorX;
        private int cursorY;

        // Если фигура выбрана, стрелки перемещают её
        private Shape selectedShape = null;

        // Сколько строк внизу зарезервируем под меню/ввод
        private const int MENU_LINES = 8;

        public Terminal(int canvasWidth, int canvasHeight)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;

            // Инициализируем Canvas, где хранятся и рисуются фигуры
            canvas = new Canvas(canvasWidth, canvasHeight);

            cursorX = 0;
            cursorY = 0;
        }

        public void Run()
        {
            Console.Clear();

            // Canvas сам рисует рамку и может хранить список фигур
            canvas.DrawFrame();
            canvas.RedrawAllShapes();
            DrawMenu();
            DrawCursor();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Выбор/снятие выбора фигуры
                    if (selectedShape == null)
                    {
                        selectedShape = GetShapeAtCursor();
                        if (selectedShape != null)
                        {
                            PrintMessage("Фигура выбрана. Стрелки перемещают её. Нажмите Enter для отмены выбора.");
                        }
                        else
                        {
                            PrintMessage("Фигура не найдена под курсором.");
                        }
                    }
                    else
                    {
                        selectedShape = null;
                        PrintMessage("Выбор снят. Стрелки перемещают курсор.");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.X)
                {
                    // Удаляем выбранную фигуру
                    if (selectedShape != null)
                    {
                        canvas.RemoveShape(selectedShape);
                        selectedShape = null;
                        PrintMessage("Выбранная фигура удалена.");
                        canvas.RedrawAllShapes();
                    }
                    else
                    {
                        PrintMessage("Нет выбранной фигуры для удаления.");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.D)
                {
                    // Показываем меню добавления фигур
                    ShowAddShapeMenu();
                }
                else if (IsArrowKey(keyInfo.Key))
                {
                    int dx = 0, dy = 0;
                    if (keyInfo.Key == ConsoleKey.UpArrow) dy = -1;
                    if (keyInfo.Key == ConsoleKey.DownArrow) dy = 1;
                    if (keyInfo.Key == ConsoleKey.LeftArrow) dx = -1;
                    if (keyInfo.Key == ConsoleKey.RightArrow) dx = 1;

                    if (selectedShape != null)
                    {
                        // Перемещаем выбранную фигуру
                        // (Стираем старые пиксели, фигура сама пересчитает и перерисует себя при RedrawAllShapes)
                        EraseShape(selectedShape);
                        selectedShape.Move(dx, dy);
                        canvas.RedrawAllShapes();
                    }
                    else
                    {
                        // Перемещаем курсор
                        MoveCursor(dx, dy);
                    }
                }
            }
        }

        /// <summary>
        /// Меню для добавления новой фигуры: линия, точка, прямоугольник.
        /// </summary>
        private void ShowAddShapeMenu()
        {
            // Снимаем выбор, чтобы стрелки не двигали выбранную фигуру
            selectedShape = null;

            ClearMenuArea();
            PrintMessage("Добавить фигуру: [1] Линия, [2] Точка, [3] Прямоугольник");
            string choice = ReadLineAt(canvasHeight + 3);

            switch (choice)
            {
                case "1":
                    if (PromptLineInput(out int x1, out int y1, out int x2, out int y2,
                                        out char lineSym, out ConsoleColor lineColor))
                    {
                        // Вызываем методы Canvas для добавления
                        canvas.AddLine(x1, y1, x2, y2, lineSym, lineColor);
                    }
                    break;
                case "2":
                    if (PromptPointInput(out int px, out int py, out char pSym, out ConsoleColor pColor))
                    {
                        canvas.AddPoint(px, py, pSym, pColor);
                    }
                    break;
                case "3":
                    if (PromptRectangleInput(out int rx1, out int ry1, out int rx2, out int ry2,
                                             out char rSym, out ConsoleColor rColor))
                    {
                        canvas.AddRectangle(rx1, ry1, rx2, ry2, rSym, rColor);
                    }
                    break;
                default:
                    PrintMessage("Неверный выбор. Нажмите Enter.");
                    ReadLineAt(canvasHeight + 3);
                    break;
            }

            // Перерисовываем
            canvas.RedrawAllShapes();
            DrawMenu();
            DrawCursor();
        }

        /// <summary>
        /// Запрашивает у пользователя параметры для линии (x1,y1,x2,y2, символ, цвет).
        /// Возвращает true, если ввод корректен.
        /// </summary>
        private bool PromptLineInput(out int x1, out int y1, out int x2, out int y2,
                                     out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Введите X1:");
            if (!TryReadInt(out x1)) return false;

            PrintMessage("Введите Y1:");
            if (!TryReadInt(out y1)) return false;

            PrintMessage("Введите X2:");
            if (!TryReadInt(out x2)) return false;

            PrintMessage("Введите Y2:");
            if (!TryReadInt(out y2)) return false;

            PrintMessage("Символ для линии (Enter=*)");
            string symInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            return true;
        }

        /// <summary>
        /// Запрашивает координаты (x, y), символ и цвет для точки.
        /// </summary>
        private bool PromptPointInput(out int x, out int y, out char symbol, out ConsoleColor color)
        {
            x = y = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Введите X:");
            if (!TryReadInt(out x)) return false;

            PrintMessage("Введите Y:");
            if (!TryReadInt(out y)) return false;

            PrintMessage("Символ точки (Enter=*)");
            string symInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            return true;
        }

        /// <summary>
        /// Запрашивает координаты (x1,y1,x2,y2), символ и цвет для прямоугольника.
        /// </summary>
        private bool PromptRectangleInput(out int x1, out int y1, out int x2, out int y2,
                                          out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = 0;
            symbol = '#';
            color = ConsoleColor.White;

            PrintMessage("Введите X1 (левый верх):");
            if (!TryReadInt(out x1)) return false;

            PrintMessage("Введите Y1 (левый верх):");
            if (!TryReadInt(out y1)) return false;

            PrintMessage("Введите X2 (правый низ):");
            if (!TryReadInt(out x2)) return false;

            PrintMessage("Введите Y2 (правый низ):");
            if (!TryReadInt(out y2)) return false;

            PrintMessage("Символ прямоугольника (Enter=#)");
            string symInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            return true;
        }

        /// <summary>
        /// Рисует меню внизу (строка canvasHeight+2).
        /// </summary>
        private void DrawMenu()
        {
            int row = canvasHeight + 2;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine("Меню: [D] - добавить фигуру, [Enter] - выбрать/снять выбор, [X] - удалить, [Esc] - выход");
        }

        /// <summary>
        /// Стирает пиксели выбранной фигуры (заменяет их пробелами).
        /// </summary>
        private void EraseShape(Shape shape)
        {
            foreach (var p in shape.OuterPixels)
            {
                canvas.SetPixel(p.X, p.Y, ' ', ConsoleColor.Black);
            }
            foreach (var p in shape.InnerPixels)
            {
                canvas.SetPixel(p.X, p.Y, ' ', ConsoleColor.Black);
            }
        }

        /// <summary>
        /// Находит фигуру под курсором.
        /// </summary>
        private Shape GetShapeAtCursor()
        {
            var allShapes = canvas.GetShapes();
            foreach (var s in allShapes)
            {
                if (s.ContainsPoint(cursorX, cursorY))
                    return s;
            }
            return null;
        }

        /// <summary>
        /// Проверяет, является ли key – стрелкой.
        /// </summary>
        private bool IsArrowKey(ConsoleKey key)
        {
            return (key == ConsoleKey.UpArrow ||
                    key == ConsoleKey.DownArrow ||
                    key == ConsoleKey.LeftArrow ||
                    key == ConsoleKey.RightArrow);
        }

        /// <summary>
        /// Выводит сообщение на строке (canvasHeight + 4), очищая её.
        /// </summary>
        private void PrintMessage(string msg)
        {
            int row = canvasHeight + 4;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Очищает N строк под холстом, если нужно (в данном случае используем ClearLine выборочно).
        /// </summary>
        private void ClearMenuArea()
        {
            int startRow = canvasHeight + 2;
            for (int i = 0; i < MENU_LINES; i++)
            {
                ClearLine(startRow + i);
            }
        }

        /// <summary>
        /// Очищает строку, записывая пробелы.
        /// </summary>
        private void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', 120));
            Console.SetCursorPosition(0, row);
        }

        /// <summary>
        /// Считывает одно целое число. Возвращает false, если ввод некорректен.
        /// </summary>
        private bool TryReadInt(out int result)
        {
            FlushInput();
            int row = canvasHeight + 5;
            string input = ReadLineAt(row);
            if (!int.TryParse(input, out result))
            {
                PrintMessage("Ошибка ввода (не целое число)!");
                ReadLineAt(row);  // Ждём Enter
                return false;
            }
            return true;
        }

        /// <summary>
        /// Считывает строку на указанной строке (row) и очищает её после ввода.
        /// </summary>
        private string ReadLineAt(int row)
        {
            Console.SetCursorPosition(0, row);
            string input = Console.ReadLine();
            ClearLine(row);
            return input;
        }

        /// <summary>
        /// Сбрасывает буфер консоли (удаляя лишние нажатия).
        /// </summary>
        private void FlushInput()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
        /// <summary>
        /// Рисует курсор в виде подчёркивания ('_') жёлтым цветом.
        /// </summary>
        private void DrawCursor()
        {
            // Поскольку рамка занимает 1 строку/столбец сверху и слева,
            // реальные координаты курсора в консоли = (cursorX + 1, cursorY + 1).
            int drawX = cursorX + 1;
            int drawY = cursorY + 1;

            // Сохраняем текущую позицию курсора (не обязательно).
            int prevLeft = Console.CursorLeft;
            int prevTop = Console.CursorTop;

            Console.SetCursorPosition(drawX, drawY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("_");  // Символ курсора
            Console.ForegroundColor = ConsoleColor.White;

            // Возвращаем курсор консоли на прежнее место (не обязательно).
            Console.SetCursorPosition(prevLeft, prevTop);
        }

        /// <summary>
        /// Перемещает курсор на dx, dy, стирая старое его положение и рисуя новое.
        /// </summary>
        private void MoveCursor(int dx, int dy)
        {
            // Сначала стираем старое положение курсора,
            // восстанавливая символ пикселя из холста (Canvas).
            EraseCursor();

            // Обновляем координаты, ограничивая их пределами холста.
            cursorX = Math.Max(0, Math.Min(cursorX + dx, canvasWidth - 1));
            cursorY = Math.Max(0, Math.Min(cursorY + dy, canvasHeight - 1));

            // Рисуем курсор в новой позиции.
            DrawCursor();
        }

        private void EraseCursor()
        {
            // Получаем пиксель, который там был
            Pixel oldPixel = canvas.GetPixel(cursorX, cursorY);
            Console.SetCursorPosition(cursorX + 1, cursorY + 1);
            Console.ForegroundColor = oldPixel.Color;
            Console.Write(oldPixel.Symbol);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
