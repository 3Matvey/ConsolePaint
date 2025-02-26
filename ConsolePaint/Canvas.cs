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

    // Метод для сдвига фигуры на холсте
    public void MoveShape(Shape shape, int dx, int dy)
    {
        // Сдвигаем все пиксели фигуры
        foreach (var pixel in  shape.OuterPixels)
        {
            pixel.X++; //+= dx;
            pixel.Y += dy;
        }

        foreach (var pixel in shape.InnerPixels)
        {
            pixel.X += dx;
            pixel.Y += dy;
        }

        // После сдвига пересчитываем пиксели фигуры
        shape.CalculatePixels();
        Draw(shape);  // Перерисовываем фигуру на холсте с новыми координатами
    }

    // Метод для заливки фигуры
    public void FillShape(Shape shape, char fillSymbol)
    {
        // Заполняем внутренние пиксели новой заливкой
        foreach (var pixel in shape.InnerPixels)
        {
            SetPixel(pixel.X, pixel.Y, fillSymbol, pixel.Color);
        }
    }

    // Метод для установки пикселя на холсте
    private void SetPixel(int x, int y, char symbol, ConsoleColor color)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            pixels[x, y] = new Pixel(x, y, symbol, color);
            Console.SetCursorPosition(x, y);
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
                Console.SetCursorPosition(i, j);
                Console.Write(' ');
            }
        }
    }
}
