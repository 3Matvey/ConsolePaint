using ConsolePaint.Commands;
using ConsolePaint.Services;

namespace ConsolePaint.Terminal
{
    public partial class Terminal
    {
        private readonly Canvas canvas;
        private readonly UndoManager undoManager;

        private readonly int canvasWidth;
        private readonly int canvasHeight;

        private int cursorX;
        private int cursorY;

        private Shape? selectedShape;

        private const int MenuLines = 8;

        public Terminal()
        {
            canvasWidth = Console.WindowWidth - 10;
            canvasHeight = Console.WindowHeight - 10;

            canvas = new Canvas(canvasWidth, canvasHeight);
            undoManager = new UndoManager();

            cursorX = 0;
            cursorY = 0;
        }

        public Terminal(string fileName) 
            : this()
        {
            LoadCanvas(fileName);
        }

        public void Run()
        {
            Console.Clear();
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
                    if (selectedShape == null)
                    {
                        selectedShape = GetShapeAtCursor();
                        PrintMessage(selectedShape is not null
                            ? "Фигура выбрана. Стрелки перемещают её. [X] - удалить. [F] - заливка. Нажмите Enter для отмены выбора."
                            : "Фигура не найдена под курсором.");
                    }
                    else
                    {
                        selectedShape = null;
                        PrintMessage("Выбор снят. Стрелки перемещают курсор.");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Z)  // Undo
                {
                    undoManager.Undo();
                }
                else if (keyInfo.Key == ConsoleKey.Y)  // Redo
                {
                    undoManager.Redo();
                }
                else if (keyInfo.Key == ConsoleKey.X)
                {
                    if (selectedShape != null)
                    {
                        var addAction = new RemoveShapeAction(canvas, selectedShape);
                        undoManager.ExecuteAction(addAction);
                        selectedShape = null;
                        PrintMessage("Выбранная фигура удалена.");
                    }
                    else
                    {
                        PrintMessage("Нет выбранной фигуры для удаления.");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.D)
                {
                    ShowAddShapeMenu();
                }
                else if (keyInfo.Key == ConsoleKey.F)
                {
                    if (selectedShape != null)
                    {
                        PrintMessage("Введите символ заливки (Enter = +):");
                        string fillSym = ReadLineAt(canvasHeight + 5);
                        char fillSymbol = string.IsNullOrEmpty(fillSym) ? '+' : fillSym[0];

                        PrintMessage("Введите цвет заливки (например, Blue, Enter = White):");
                        string fillCol = ReadLineAt(canvasHeight + 5);
                        ConsoleColor fillColor = Enum.TryParse(fillCol, true, out fillColor) ? fillColor : ConsoleColor.White;

                        var fillAction = new FillShapeAction(canvas, selectedShape, fillSymbol, fillColor);
                        undoManager.ExecuteAction(fillAction);

                        PrintMessage("Заливка применена. Дважды Нажмите Enter.");
                        ReadLineAt(canvasHeight + 5);
                    }
                    else
                    {
                        PrintMessage("Нет выбранной фигуры для заливки.");
                        ReadLineAt(canvasHeight + 5);
                    }
                }
                else if (keyInfo.Key == ConsoleKey.S)
                {
                    SaveCanvas();
                }
                else if (keyInfo.Key == ConsoleKey.L)
                {
                    LoadCanvas();
                }
                else if (IsArrowKey(keyInfo.Key))
                {
                    int dx = 0, dy = 0;
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow: dy = -1; break;
                        case ConsoleKey.DownArrow: dy = 1; break;
                        case ConsoleKey.LeftArrow: dx = -1; break;
                        case ConsoleKey.RightArrow: dx = 1; break;
                    }

                    if (selectedShape != null)
                    {
                        var moveAction = new MoveShapeAction(canvas, selectedShape, dx, dy);
                        undoManager.ExecuteAction(moveAction);
                    }
                    else
                    {
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
            PrintMessage("Добавить фигуру: [1] Линия, [2] Точка, [3] Прямоугольник, [4] Эллипс, [5] Треугольник");
            string choice = ReadLineAt(canvasHeight + 5);
            Shape s = null!;
            switch (choice)
            {
                case "1":
                    if (PromptLineInput(out int x1, out int y1, out int x2, out int y2, out char lineSym, out ConsoleColor lineColor))
                    {
                        s = ShapeFactory.CreateLine(x1, y1, x2, y2, lineSym, lineColor);
                       // canvas.AddShape(s);
                    }
                    break;
                case "2":
                    if (PromptPointInput(out int px, out int py, out char pSym, out ConsoleColor pColor))
                    {
                        s = ShapeFactory.CreatePoint(px, py, pSym, pColor);
                        //canvas.AddShape(s);
                    }
                    break;
                case "3":
                    if (PromptRectangleInput(out int rx1, out int ry1, out int rx2, out int ry2, out char rSym, out ConsoleColor rColor))
                    {
                        s = ShapeFactory.CreateRectangle(rx1, ry1, rx2, ry2, rSym, rColor);
                       // canvas.AddShape(s);
                    }
                    break;
                case "4":
                    if (PromptEllipseInput(out int ex, out int ey, out int exRadius, out int eyRadius, out char eSym, out ConsoleColor eColor))
                    {
                        s = ShapeFactory.CreateEllipse(ex, ey, exRadius, eyRadius, eSym, eColor);
                        //canvas.AddShape(s);
                    }
                    break;
                case "5":
                    if (PromptTriangleInput(out int tx1, out int ty1, out int tx2, out int ty2, out int tx3, out int ty3, out char tSym, out ConsoleColor tColor))
                    {
                        s = ShapeFactory.CreateTriangle(tx1, ty1, tx2, ty2, tx3, ty3, tSym, tColor);
                       // canvas.AddShape(s);
                    }
                    break;
                default:
                    PrintMessage("Неверный выбор. Дважды Нажмите Enter.");
                    ReadLineAt(canvasHeight + 5);
                    break;
            }

            if (s != null)
            {
                var addAction = new AddShapeAction(canvas, s);
                undoManager.ExecuteAction(addAction);
            }

            // Перерисовываем
            canvas.RedrawAllShapes();
            DrawMenu();
            DrawCursor();
        }

        
        /// <summary>
        /// Рисует меню внизу (строка canvasHeight+2).
        /// </summary>
        private void DrawMenu()
        {
            int row = canvasHeight + 2;
            ClearLine(row);
            Console.SetCursorPosition(0, row);
            Console.WriteLine("Меню: [D] - добавить фигуру, [S] - сохранить, [L] - загрузить, [Enter] - выбрать/снять выбор, [Esc] - выход");
        }

        

        /// <summary>
        /// Находит фигуру под курсором.
        /// </summary>
        private Shape? GetShapeAtCursor()
        {
            var allShapes = canvas.Shapes;
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
        private static bool IsArrowKey(ConsoleKey key)
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
            for (int i = 0; i < MenuLines; i++)
            {
                ClearLine(startRow + i);
            }
        }

        /// <summary>
        /// Очищает строку, записывая пробелы.
        /// </summary>
        private static void ClearLine(int row)
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
                ReadLineAt(row);  // Ждать Enter
                return false;
            }
            return true;
        }

        /// <summary>
        /// Считывает строку на указанной строке (row) и очищает её после ввода.
        /// </summary>
        private static string ReadLineAt(int row)
        {
            Console.SetCursorPosition(0, row);
            string input = Console.ReadLine();
            ClearLine(row);
            return input;
        }

        /// <summary>
        /// Сбрасывает буфер консоли (удаляя лишние нажатия).
        /// </summary>
        private static void FlushInput()
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
