namespace ConsolePaint;

public class Canvas
{
    private int width;
    private int height;
    private Pixel[,] pixels;

    public Canvas(int width, int height)
    {
        this.width = width;
        this.height = height;
        pixels = new Pixel[width, height];  // Инициализация холста
    }

    // Метод для рисования фигуры на холсте
    public void Draw(Shape shape)
    {
        // Рисуем внешние пиксели (OuterPixels)
        foreach (var pixel in shape.OuterPixels)
        {
            SetPixel(pixel.X, pixel.Y, pixel.Symbol, pixel.Color);
        }

        // Рисуем внутренние пиксели (InnerPixels)
        foreach (var pixel in shape.InnerPixels)
        {
            SetPixel(pixel.X, pixel.Y, pixel.Symbol, pixel.Color);
        }
    }

    // Метод для отрисовки рамки
    public void DrawFrame()
    {
        // Рисуем верхнюю границу с номерами колонок
        Console.SetCursorPosition(0, 4); // Отступим 4 строки для меню
        Console.Write("  ");  // Отступ для координат
        for (int i = 0; i < width; i++)
        {
            Console.Write($"{i % 10}");  // Номера колонок
        }
        Console.WriteLine();

        // Рисуем левую границу с номерами строк
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(0, y + 5); // Отступим на 5 строк для меню и верхней границы
            Console.Write($"{y % 10} ");  // Номера строк
            for (int x = 0; x < width; x++)
            {
                Console.Write(" ");  // Пустое место для фигуры
            }
        }

        // Рисуем нижнюю границу с номерами колонок
        Console.SetCursorPosition(0, height + 5);
        Console.Write("  ");
        for (int i = 0; i < width; i++)
        {
            Console.Write($"{i % 10}");  // Номера колонок
        }
        Console.WriteLine();
    }

    // Рисуем все фигуры
    public void DrawShapes(List<Shape> shapes)
    {
        foreach (var shape in shapes)
        {
            Draw(shape);  // Рисуем каждую фигуру
        }
    }

    // Метод для установки пикселя на холсте
    private void SetPixel(int x, int y, char symbol, ConsoleColor color)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            pixels[x, y] = new Pixel(x, y, symbol, color);
            Console.SetCursorPosition(x + 1, y + 5);  // Смещаем на 1, чтобы не затереть границу
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }
    }

    // Очистка холста
    public void Clear()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                pixels[i, j] = new Pixel(i, j, ' ', ConsoleColor.Black);  // Инициализация пустыми пикселями
                Console.SetCursorPosition(i + 1, j + 5);  // Смещаем на 1, чтобы не затереть границу
                Console.Write(' ');
            }
        }
    }
}

