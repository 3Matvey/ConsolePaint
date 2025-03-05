using System.Globalization;

namespace ConsolePaint
{
    public partial class Terminal
    {
        private bool PromptEllipseInput(out int x, out int y, out int radiusX, out int radiusY, out char symbol, out ConsoleColor color)
        {
            x = y = radiusX = radiusY = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Введите координаты центра (X):");
            if (!TryReadInt(out x)) return false;

            PrintMessage("Введите координаты центра (Y):");
            if (!TryReadInt(out y)) return false;
            TempPointDraw(x, y, out Shape tempPoint);

            PrintMessage("Введите радиус по X:");
            if (!TryReadInt(out radiusX)) return false;

            PrintMessage("Введите радиус по Y:");
            if (!TryReadInt(out radiusY)) return false;

            PrintMessage("Символ для эллипса (Enter=*)");
            string symInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            TempPointsRemove([tempPoint]);
            return true;
        }

        private bool PromptTriangleInput(out int x1, out int y1, out int x2, out int y2, out int x3, out int y3, out char symbol, out ConsoleColor color)
        {
            x1 = y1 = x2 = y2 = x3 = y3 = 0;
            symbol = '*';
            color = ConsoleColor.White;

            PrintMessage("Введите координаты первой вершины (X1 Y1):");
            if (!TryReadInt(out x1)) return false;
            if (!TryReadInt(out y1)) return false;
            TempPointDraw(x1, y1, out Shape tempPoint1);

            PrintMessage("Введите координаты второй вершины (X2 Y2):");
            if (!TryReadInt(out x2)) return false;
            if (!TryReadInt(out y2)) return false;
            TempPointDraw(x2, y2, out Shape tempPoint2);

            PrintMessage("Введите координаты третьей вершины (X3 Y3):");
            if (!TryReadInt(out x3)) return false;
            if (!TryReadInt(out y3)) return false;
            TempPointDraw(x3, y3, out Shape tempPoint3);

            PrintMessage("Символ для треугольника (Enter=*)");
            string symInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 4);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }
            TempPointsRemove([tempPoint1, tempPoint2, tempPoint3]);

            return true;
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

            TempPointDraw(x1, y1, out Shape tempPoint1);

            PrintMessage("Введите X2:");
            if (!TryReadInt(out x2)) return false;

            PrintMessage("Введите Y2:");
            if (!TryReadInt(out y2)) return false;

            TempPointDraw(x2, y2, out Shape tempPoint2);

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

            TempPointsRemove([tempPoint1, tempPoint2]);

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
            string symInput = ReadLineAt(canvasHeight + 5);  //было 4
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
            x1 = y1 = x2 = y2 = 0;
            symbol = '#';
            color = ConsoleColor.White;

            PrintMessage("Введите X1 (левый верх):");
            if (!TryReadInt(out x1)) return false;

            PrintMessage("Введите Y1 (левый верх):");
            if (!TryReadInt(out y1)) return false;

            TempPointDraw(x1, y1, out Shape tempPoint1);

            PrintMessage("Введите X2 (правый низ):");
            if (!TryReadInt(out x2)) return false;

            PrintMessage("Введите Y2 (правый низ):");
            if (!TryReadInt(out y2)) return false;

            TempPointDraw(x2, y2, out Shape tempPoint2);

            PrintMessage("Символ прямоугольника (Enter=#)");
            string symInput = ReadLineAt(canvasHeight + 5);  //было 4
            if (!string.IsNullOrEmpty(symInput)) symbol = symInput[0];

            PrintMessage("Цвет (Enter=White)");
            string colInput = ReadLineAt(canvasHeight + 5);
            if (!string.IsNullOrEmpty(colInput))
            {
                if (!Enum.TryParse(colInput, true, out color))
                    color = ConsoleColor.White;
            }

            TempPointsRemove([tempPoint1, tempPoint2]);

            return true;
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
            FileManager.SaveShapesToFile(canvas.GetShapes(), filename);
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