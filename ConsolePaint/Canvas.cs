using ConsolePaint;

public class Canvas
{
    private int width;
    private int height;
    private Pixel[,] pixels;

    public Canvas(int width, int height)
    {
        this.width = width;
        this.height = height;
        pixels = new Pixel[width, height];

        DrawFrame();
    }

    /// <summary>
    /// Рисуем ASCII-рамку:
    ///  - Верхняя граница на строке 0
    ///  - Внутренняя область строк: 1..height
    ///  - Нижняя граница на строке (height + 1)
    ///  - Левая граница на столбце 0, правая — (width + 1)
    /// Итого реальных строк в консоли = height + 2, столбцов = width + 2.
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
            // Пустая область внутри
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

    // Отрисовка уже созданных фигур
    public void DrawShapes(List<Shape> shapes)
    {
        foreach (var shape in shapes)
        {
            Draw(shape);
        }
    }

    // Отрисовка одной фигуры
    public void Draw(Shape shape)
    {
        // Внешние пиксели
        foreach (var p in shape.OuterPixels)
        {
            SetPixel(p.X, p.Y, p.Symbol, p.Color);
        }
        // Внутренние пиксели
        foreach (var p in shape.InnerPixels)
        {
            SetPixel(p.X, p.Y, p.Symbol, p.Color);
        }
    }

    // Устанавливаем пиксель (x,y) внутри рамки
    public void SetPixel(int x, int y, char symbol, ConsoleColor color)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            // Создаём объект пикселя и записываем в массив
            pixels[x, y] = new Pixel(x, y, symbol, color);

            // Смещаем курсор в консоли (x+1, y+1), чтобы не затирать рамку
            Console.SetCursorPosition(x + 1, y + 1);
            Console.ForegroundColor = color;
            Console.Write(symbol);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    // Очистка холста (внутренней области)
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
    public Pixel GetPixel(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            // Если в массиве ещё нет пикселя, вернём «пустой»
            if (pixels[x, y] == null)
            {
                return new Pixel(x, y, ' ', ConsoleColor.Black);
            }
            return pixels[x, y];
        }
        // Если вышли за границы холста — тоже возвращаем «пустой»
        return new Pixel(x, y, ' ', ConsoleColor.Black);
    }

}
