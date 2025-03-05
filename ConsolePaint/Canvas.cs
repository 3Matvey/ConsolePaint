namespace ConsolePaint
{
    public class Canvas
    {
        private int width;
        private int height;
        private Pixel[,] pixels;
        private List<Shape> shapes;  // Список фигур, добавляемых через методы AddShape и т.д.

        public int Height => height;
        public int Width => width;
        
        public List<Shape> Shapes => shapes;

        public Canvas(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new Pixel[width, height];
            shapes = new List<Shape>();

            //DrawFrame();
        }

        /// <summary>
        /// Рисует ASCII-рамку вокруг внутренней области холста.
        /// </summary>
        public void DrawFrame()
        {
            // Верхняя граница
            Console.SetCursorPosition(0, 0);
            Console.Write(0);
            for (int i = 0; i < width; i++)
            {
                Console.Write("_");
            }
            Console.Write(width);
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
            Console.Write(height);
            for (int i = 0; i < width; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Отрисовывает фигуру, выводя только её контур (внешние пиксели).
        /// </summary>
        public void Draw(Shape shape)
        {
            foreach (var p in shape.OuterPixels)
            {
                SetPixel(p.X, p.Y, p.Symbol, p.Color);
            }
        }

        /// <summary>
        /// Заливает фигуру, отрисовывая её внутренние пиксели.
        /// </summary>
        public void Fill(Shape shape)
        {
            foreach (var p in shape.InnerPixels)
            {
                SetPixel(p.X, p.Y, p.Symbol, p.Color);
            }
        }

        /// <summary>
        /// Очищает внутреннюю область холста (не затрагивая рамку).
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixels[x, y] = new Pixel(x, y, ' ', ConsoleColor.Black);
                    Console.SetCursorPosition(x + 1, y + 1);
                    Console.Write(' ');
                }
            }
        }

        /// <summary>
        /// Добавляет фигуру в список и сразу отрисовывает её контур.
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
        /// Перерисовывает все фигуры (выводит контуры всех фигур).
        /// </summary>
        public void RedrawAllShapes()
        {
            Clear();
            foreach (var shape in shapes)
            {
                Draw(shape);

                if (shape.InnerPixels.Any(p => p.Symbol != ' '))
                {
                    foreach (var p in shape.InnerPixels)
                    {
                        SetPixel(p.X, p.Y, p.Symbol, p.Color);
                    }
                }
            }
        }


        /// <summary>
        /// Устанавливает пиксель в указанной точке внутренней области холста.
        /// Смещает координаты на (1,1), чтобы не затереть рамку.
        /// </summary>
        public void SetPixel(int x, int y, char symbol, ConsoleColor color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                pixels[x, y] = new Pixel(x, y, symbol, color);
                Console.SetCursorPosition(x + 1, y + 1);
                Console.ForegroundColor = color;
                Console.Write(symbol);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Возвращает пиксель по координатам. Если его нет, возвращает "пустой" пиксель.
        /// </summary>
        public Pixel GetPixel(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return pixels[x, y] ?? new Pixel(x, y, ' ', ConsoleColor.Black);
            }
            return new Pixel(x, y, ' ', ConsoleColor.Black);
        }
    }
}
