using ConsolePaint.Services;
using System.Globalization;

namespace ConsolePaint.Terminal
{
    public partial class Terminal
    {
        private bool PromptEllipseInput(out int x, out int y, out int radiusX, out int radiusY, out char symbol, out ConsoleColor color)
        {
            x = y = radiusX = radiusY = 0;
            symbol = '*';
            color = ConsoleColor.White;
            Shape tempPoint = null!;
            try
            {
                // Проверяем, что координаты центра входят в холст
                PrintMessage("Введите координаты центра (X):");
                if (!TryReadIntInRange(out x, 0, canvasWidth - 1)) return false;

                PrintMessage("Введите координаты центра (Y):");
                if (!TryReadIntInRange(out y, 0, canvasHeight - 1)) return false;
                TempPointDraw(x, y, out tempPoint);

                // Читаем радиус по X и проверяем, чтобы фигура не выходила за пределы холста
                PrintMessage("Введите радиус по X:");
                if (!TryReadIntInRange(out radiusX, 1, canvasWidth / 2 - 1)) return false;
                int maxRadiusX = Math.Min(x, canvasWidth - 1 - x);
                if (radiusX < 1 || radiusX > maxRadiusX)
                {
                    PrintMessage($"Радиус по X должен быть от 1 до {maxRadiusX}.");
                    return false;
                }

                // Читаем радиус по Y и аналогичная проверка
                PrintMessage("Введите радиус по Y:");
                if (!TryReadIntInRange(out radiusY, 1, canvasHeight / 2 - 1)) return false;
                int maxRadiusY = Math.Min(y, canvasHeight - 1 - y);
                if (radiusY < 1 || radiusY > maxRadiusY)
                {
                    PrintMessage($"Радиус по Y должен быть от 1 до {maxRadiusY}.");
                    return false;
                }

                PrintMessage("Символ для эллипса (Enter=*)");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Цвет (Enter=White)");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }

                return true;
            }

            finally
            {
                TempPointsRemove([tempPoint]);
            }
        }


        private bool PromptTriangleInput(out int x1, out int y1, out int x2, out int y2, out int x3, out int y3, out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = x3 = y3 = 0;
            symbol = '*';
            color = ConsoleColor.White;
            Shape tempPoint1, tempPoint2, tempPoint3;
            tempPoint1 = tempPoint2 = tempPoint3 = null!;
            try
            {
                PrintMessage("Введите координаты первой вершины (X1):");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;
                PrintMessage("Введите координаты первой вершины (Y1):");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Введите координаты второй вершины (X2):");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;
                PrintMessage("Введите координаты второй вершины (Y2):");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Введите координаты третьей вершины (X3):");
                if (!TryReadIntInRange(out x3, 0, canvasWidth - 1)) return false;
                PrintMessage("Введите координаты третьей вершины (Y3):");
                if (!TryReadIntInRange(out y3, 0, canvasHeight - 1)) return false;
                TempPointDraw(x3, y3, out tempPoint3);

                PrintMessage("Символ для треугольника (Enter=*)");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Цвет (Enter=White)");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }
                return true;
            }
            finally
            {
                TempPointsRemove([tempPoint1, tempPoint2, tempPoint3]);
            }
        }


        /// <summary>
        /// Запрашивает у пользователя параметры для линии (x1,y1,x2,y2, символ, цвет).
        /// Возвращает true, если ввод корректен.
        /// </summary>
        private bool PromptLineInput(out int x1, out int y1, out int x2, out int y2,
                               out char symbol, out ConsoleColor color)
        {
            Shape tempPoint1, tempPoint2;
            tempPoint1 = tempPoint2 = null!;
            x1 = y1 = x2 = y2 = 0;
            symbol = '*';
            color = ConsoleColor.White;
            try
            {
                PrintMessage("Введите X1:");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;

                PrintMessage("Введите Y1:");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Введите X2:");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;

                PrintMessage("Введите Y2:");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Символ для линии (Enter=*)");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Цвет (Enter=White)");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }
                return true;
            }
            finally
            {
                TempPointsRemove([tempPoint1, tempPoint2]);
            }
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
            if (!TryReadIntInRange(out x, 0, canvasWidth - 1)) return false;

            PrintMessage("Введите Y:");
            if (!TryReadIntInRange(out y, 0, canvasHeight - 1)) return false;

            PrintMessage("Символ точки (Enter=*)");
            string symInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 5);
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
            x1 = y1 = x2 = y2 = default;
            symbol = '#';
            color = ConsoleColor.White;
            Shape tempPoint1, tempPoint2;
            tempPoint1 = tempPoint2 = null!;
            try
            {
                PrintMessage("Введите X первой вершины:");
                if (!TryReadIntInRange(out x1, 0, canvasWidth - 1)) return false;

                PrintMessage("Введите Y первой вершины:");
                if (!TryReadIntInRange(out y1, 0, canvasHeight - 1)) return false;
                TempPointDraw(x1, y1, out tempPoint1);

                PrintMessage("Введите X второй вершины:");
                if (!TryReadIntInRange(out x2, 0, canvasWidth - 1)) return false;

                PrintMessage("Введите Y второй вершины:");
                if (!TryReadIntInRange(out y2, 0, canvasHeight - 1)) return false;
                TempPointDraw(x2, y2, out tempPoint2);

                PrintMessage("Символ прямоугольника (Enter=#)");
                string symInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

                PrintMessage("Цвет (Enter=White)");
                string colInput = ReadLineAt(canvasHeight + 5);
                if (!string.IsNullOrEmpty(colInput))
                {
                    if (!Enum.TryParse(colInput, true, out color))
                        color = ConsoleColor.White;
                }
                return true;
            }
            finally
            {
                TempPointsRemove([tempPoint1, tempPoint2]);
            }
        }

        /// <summary>
        /// Считывает целое число и проверяет, что оно находится в диапазоне [min, max].
        /// Если число вне диапазона, выводит сообщение и возвращает false.
        /// </summary>
        private bool TryReadIntInRange(out int result, int min, int max)
        {
            if (!TryReadInt(out result))
                return false;
            if (result < min || result > max)
            {
                PrintMessage($"Значение должно быть от {min} до {max}.");
                return false;
            }
            return true;

            bool TryReadInt(out int result)
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
        }


        private void TempPointDraw(int x, int y, out Shape tempPoint)
        {
            tempPoint = ShapeFactory.CreatePoint(x, y, '*', ConsoleColor.Red);
            canvas.AddShape(tempPoint);
        }

        private void TempPointsRemove(Shape[] shapes)
        {
            foreach (var s in shapes)
            {
                canvas.RemoveShape(s);
            }
        }


        // Новый метод для сохранения холста:
        private void SaveCanvas()
        {
            PrintMessage("Введите имя файла для сохранения (например, canvas):");
            string filename = ReadLineAt(canvasHeight + 5);
            if (!filename.EndsWith(".json"))
            {
                filename += ".json";
            }
            FileManager.SaveShapesToFile(canvas.Shapes, filename);
            PrintMessage("Холст сохранен в " + filename + ". Нажмите Enter.");
            ReadLineAt(canvasHeight + 5);
        }
        // Новый метод для загрузки холста:
        private void LoadCanvas()
        {
            PrintMessage("Введите имя файла для загрузки (например, canvas):");
            string filename = ReadLineAt(canvasHeight + 5);
            LoadCanvas(filename);
        }

        private void LoadCanvas(string filename)
        {
            if (!filename.EndsWith(".json"))
            {
                filename += ".json";
            }
            List<Shape> loadedShapes = FileManager.LoadShapesFromFile(filename);
            if (loadedShapes != null && loadedShapes.Count > 0)
            {
                //canvas.ClearShapes();
                foreach (Shape s in loadedShapes)
                {
                    canvas.AddShape(s);
                }
                canvas.RedrawAllShapes();
                PrintMessage("Холст загружен из " + filename + ". Нажмите Enter.");
                ReadLineAt(canvasHeight + 5);
            }
            else
            {
                PrintMessage("Не удалось загрузить холст из " + filename + ". Нажмите Enter.");
                ReadLineAt(canvasHeight + 5);
            }
        }
    }
}