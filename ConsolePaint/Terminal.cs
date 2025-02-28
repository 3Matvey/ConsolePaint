using System;
using System.Collections.Generic;
// Предположим, что ConsolePaint.Shapes содержит ваши классы фигур и ShapeFactory
using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Terminal
    {
        private Canvas canvas;
        private List<Shape> shapes;  // Храним все созданные фигуры

        // Размер холста
        private int canvasWidth;
        private int canvasHeight;

        // Положение курсора на холсте
        private int cursorX;
        private int cursorY;

        // Сколько строк снизу резервируем под меню
        private const int MENU_LINES = 8;

        public Terminal(int canvasWidth, int canvasHeight)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;

            canvas = new Canvas(canvasWidth, canvasHeight);
            shapes = new List<Shape>();

            // Начальные координаты курсора (0,0)
            cursorX = 0;
            cursorY = 0;
        }

        /// <summary>
        /// Запуск основного цикла работы терминала.
        /// </summary>
        public void Run()
        {
            Console.Clear();

            // Рисуем рамку холста
            canvas.DrawFrame();
            // Рисуем уже имеющиеся фигуры (если надо)
            RedrawAllShapes();

            // Рисуем меню и курсор
            DrawMenu();
            DrawCursor();

            // Основной цикл
            while (true)
            {
                // Считываем нажатие клавиши
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // Если это стрелка — двигаем курсор
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    MoveCursor(0, -1);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    MoveCursor(0, 1);
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    MoveCursor(-1, 0);
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    MoveCursor(1, 0);
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    // Выходим из приложения
                    return;
                }
                else
                {
                    // Проверим, не нажал ли пользователь цифру (1,2,3,...)
                    char c = keyInfo.KeyChar;
                    if (char.IsDigit(c))
                    {
                        HandleCommand(c.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает команду (строка), введённую пользователем (например, "1","2","3").
        /// </summary>
        private void HandleCommand(string cmd)
        {
            // Стираем курсор, чтобы не мешал выводу меню
            EraseCursor();

            switch (cmd)
            {
                case "1":
                    // Нарисовать фигуру
                    CreateFigureFlow();
                    break;
                case "2":
                    // Очистить холст
                    canvas.Clear();
                    shapes.Clear();
                    PrintMessage("Холст и список фигур очищены. Нажмите Enter.");
                    ReadLineAt(canvasHeight + 4);
                    break;
                case "3":
                    // Выход
                    PrintMessage("Выход из приложения...");
                    ReadLineAt(canvasHeight + 4);
                    Environment.Exit(0);
                    break;
                default:
                    PrintMessage("Некорректная команда. Нажмите Enter.");
                    ReadLineAt(canvasHeight + 4);
                    break;
            }

            // Перерисовываем холст и меню
            RedrawAllShapes();
            DrawMenu();
            DrawCursor();
        }

        /// <summary>
        /// Логика создания фигуры: спрашиваем тип (линия, точка, прямоугольник), 
        /// а потом координаты, символ и цвет.
        /// </summary>
        private void CreateFigureFlow()
        {
            PrintMessage("Выберите тип фигуры: [1] Линия, [2] Точка, [3] Прямоугольник");
            string choice = ReadLineAt(canvasHeight + 4);

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
                    PrintMessage("Неверный выбор типа. Нажмите Enter.");
                    ReadLineAt(canvasHeight + 4);
                    return;
            }

            if (shape != null)
            {
                shapes.Add(shape);
                canvas.Draw(shape);
                PrintMessage("Фигура создана! Нажмите Enter.");
                ReadLineAt(canvasHeight + 4);
            }
        }

        private Shape CreateLine()
        {
            PrintMessage("Введите X1:");
            if (!TryReadInt(out int x1)) return null;

            PrintMessage("Введите Y1:");
            if (!TryReadInt(out int y1)) return null;

            PrintMessage("Введите X2:");
            if (!TryReadInt(out int x2)) return null;

            PrintMessage("Введите Y2:");
            if (!TryReadInt(out int y2)) return null;

            PrintMessage("Символ линии (Enter=*)");
            string sym = ReadLineAt(canvasHeight + 4);
            char symbol = string.IsNullOrEmpty(sym) ? '*' : sym[0];

            PrintMessage("Цвет линии (Enter=White)");
            string col = ReadLineAt(canvasHeight + 4);
            ConsoleColor color = Enum.TryParse(col, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateLine(x1, y1, x2, y2, symbol, color);
        }

        private Shape CreatePoint()
        {
            PrintMessage("Введите X:");
            if (!TryReadInt(out int x)) return null;

            PrintMessage("Введите Y:");
            if (!TryReadInt(out int y)) return null;

            PrintMessage("Символ точки (Enter=*)");
            string sym = ReadLineAt(canvasHeight + 4);
            char symbol = string.IsNullOrEmpty(sym) ? '*' : sym[0];

            PrintMessage("Цвет (Enter=White)");
            string col = ReadLineAt(canvasHeight + 4);
            ConsoleColor color = Enum.TryParse(col, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreatePoint(x, y, symbol, color);
        }

        private Shape CreateRectangle()
        {
            PrintMessage("Введите X1 (левый верх):");
            if (!TryReadInt(out int x1)) return null;

            PrintMessage("Введите Y1 (левый верх):");
            if (!TryReadInt(out int y1)) return null;

            PrintMessage("Введите X2 (правый низ):");
            if (!TryReadInt(out int x2)) return null;

            PrintMessage("Введите Y2 (правый низ):");
            if (!TryReadInt(out int y2)) return null;

            PrintMessage("Символ (Enter=#)");
            string sym = ReadLineAt(canvasHeight + 4);
            char symbol = string.IsNullOrEmpty(sym) ? '#' : sym[0];

            PrintMessage("Цвет (Enter=White)");
            string col = ReadLineAt(canvasHeight + 4);
            ConsoleColor color = Enum.TryParse(col, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateRectangle(x1, y1, x2, y2, symbol, color);
        }

        /// <summary>
        /// Перерисовывает все фигуры, чтобы отобразить текущее состояние.
        /// </summary>
        private void RedrawAllShapes()
        {
            // Очистим внутреннюю часть (не рамку)
            canvas.Clear();
            // Снова нарисуем все фигуры
            foreach (var s in shapes)
            {
                canvas.Draw(s);
            }
        }

        /// <summary>
        /// Рисует меню внизу (строка canvasHeight+2).
        /// </summary>
        private void DrawMenu()
        {
            ClearMenuArea();
            int row = canvasHeight + 2;
            Console.SetCursorPosition(0, row);
            Console.WriteLine("[1] Нарисовать фигуру, [2] Очистить холст, [3] Выход");
            Console.SetCursorPosition(0, row + 1);
            Console.WriteLine("Стрелки - перемещение курсора, ESC - выход.");
            Console.SetCursorPosition(0, row + 2);
            Console.WriteLine("Нажмите цифру команды (1,2,3)...");
        }

        /// <summary>
        /// Очищает несколько строк под холстом (MENU_LINES).
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
        /// Выводит сообщение на строке (canvasHeight + 4).
        /// </summary>
        private void PrintMessage(string msg)
        {
            int row = canvasHeight + 4;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Считывает одно целое число (сброс буфера ввода + ReadLine).
        /// Если ошибка - выводим сообщение и возвращаем false.
        /// </summary>
        private bool TryReadInt(out int result)
        {
            FlushInput();
            int row = canvasHeight + 5; // Чуть ниже строки с сообщением
            string input = ReadLineAt(row);
            if (!int.TryParse(input, out result))
            {
                PrintMessage("Ошибка ввода координат (не целое число)!");
                // Дадим пользователю нажать Enter
                ReadLineAt(row);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Считывает строку на указанной строке. 
        /// После чтения сразу очищаем её, чтобы не оставался старый ввод.
        /// </summary>
        private string ReadLineAt(int row)
        {
            // Переходим в начало строки
            Console.SetCursorPosition(0, row);
            // Считываем
            string input = Console.ReadLine();
            // Очищаем строку
            ClearLine(row);
            return input;
        }

        /// <summary>
        /// Сбрасывает буфер ввода от лишних нажатых клавиш
        /// </summary>
        private void FlushInput()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        // ---------- Работа с курсором ----------

        private void MoveCursor(int dx, int dy)
        {
            EraseCursor();
            cursorX = Math.Max(0, Math.Min(cursorX + dx, canvasWidth - 1));
            cursorY = Math.Max(0, Math.Min(cursorY + dy, canvasHeight - 1));
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

        private void DrawCursor()
        {
            int drawX = cursorX + 1;
            int drawY = cursorY + 1;
            // Сохраняем позицию (не обязательно)
            int prevLeft = Console.CursorLeft;
            int prevTop = Console.CursorTop;

            Console.SetCursorPosition(drawX, drawY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("_");
            Console.ForegroundColor = ConsoleColor.White;

            // Возвращаемся назад
            Console.SetCursorPosition(prevLeft, prevTop);
        }
    }
}
