using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Создаем холст размером 20x20
        var canvas = new Canvas(20, 20);
        canvas.Clear();

        while (true)
        {
            Console.Clear();
            canvas.Display();
            Console.WriteLine("Введите команду (draw, clear, exit): ");
            string command = Console.ReadLine().ToLower();

            if (command == "exit")
            {
                break;
            }
            else if (command.StartsWith("draw"))
            {
                ProcessDrawCommand(canvas, command);
            }
            else if (command == "clear")
            {
                canvas.Clear();
            }
        }
    }

    static void ProcessDrawCommand(Canvas canvas, string command)
    {
        var parts = command.Split(' ');

        if (parts.Length < 2) return;

        string shapeType = parts[1].ToLower();
        switch (shapeType)
        {
            case "point":
                if (parts.Length == 5)
                {
                    int x = int.Parse(parts[2]);
                    int y = int.Parse(parts[3]);
                    char symbol = parts[4][0];
                    canvas.Draw(new Point(x, y, symbol));
                }
                break;
            case "line":
                if (parts.Length == 6)
                {
                    int x1 = int.Parse(parts[2]);
                    int y1 = int.Parse(parts[3]);
                    int x2 = int.Parse(parts[4]);
                    int y2 = int.Parse(parts[5]);
                    canvas.Draw(new Line(x1, y1, x2, y2, '#'));
                }
                break;
            case "rect":
                if (parts.Length == 6)
                {
                    int x1 = int.Parse(parts[2]);
                    int y1 = int.Parse(parts[3]);
                    int x2 = int.Parse(parts[4]);
                    int y2 = int.Parse(parts[5]);
                    canvas.Draw(new Rectangle(x1, y1, x2, y2, '@'));
                }
                break;
        }
    }
}

class Canvas(int width, int height)
{
    private readonly char[,] grid = new char[height, width];

    public void Clear()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = ' ';
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }
    }

    public void Draw(Shape shape)
    {
        shape.Draw(this);
    }

    public void SetPixel(int x, int y, char symbol)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[y, x] = symbol;
        }
    }
}

abstract class Shape
{
    public abstract void Draw(Canvas canvas);
}

class Point : Shape
{
    private int x, y;
    private char symbol;

    public Point(int x, int y, char symbol)
    {
        this.x = x;
        this.y = y;
        this.symbol = symbol;
    }

    public override void Draw(Canvas canvas)
    {
        canvas.SetPixel(x, y, symbol);
    }
}

class Line : Shape
{
    private int x1, y1, x2, y2;
    private char symbol;

    public Line(int x1, int y1, int x2, int y2, char symbol)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.symbol = symbol;
    }

    public override void Draw(Canvas canvas)
    {
        // Простое рисование горизонтальных или вертикальных линий
        if (x1 == x2)  // вертикальная линия
        {
            for (int y = y1; y <= y2; y++)
            {
                canvas.SetPixel(x1, y, symbol);
            }
        }
        else if (y1 == y2)  // горизонтальная линия
        {
            for (int x = x1; x <= x2; x++)
            {
                canvas.SetPixel(x, y1, symbol);
            }
        }
    }
}

class Rectangle : Shape
{
    private int x1, y1, x2, y2;
    private char symbol;

    public Rectangle(int x1, int y1, int x2, int y2, char symbol)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.symbol = symbol;
    }

    public override void Draw(Canvas canvas)
    {
        // Рисуем верхнюю и нижнюю горизонтальные линии
        for (int x = x1; x <= x2; x++)
        {
            canvas.SetPixel(x, y1, symbol);
            canvas.SetPixel(x, y2, symbol);
        }

        // Рисуем боковые вертикальные линии
        for (int y = y1; y <= y2; y++)
        {
            canvas.SetPixel(x1, y, symbol);
            canvas.SetPixel(x2, y, symbol);
        }
    }
}
