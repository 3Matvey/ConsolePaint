using ConsolePaint;
using ConsolePaint.Shapes;
class Program
{
    static void Main()
    {
        // Создаем холст размером 20x20
        var canvas = new Canvas(20, 20);
        canvas.Clear();

        //while (true)
        //{
        //    Console.Clear();
        //    canvas.Display();
        //    Console.WriteLine("Введите команду (draw, clear, exit): ");
        //    string command = Console.ReadLine().ToLower();

        //    if (command == "exit")
        //    {
        //        break;
        //    }
        //    else if (command.StartsWith("draw"))
        //    {
        //        ProcessDrawCommand(canvas, command);
        //    }
        //    else if (command == "clear")
        //    {
        //        canvas.Clear();
        //    }
        //}

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
            else if (command == "draw")
            {
                ShowDrawMenu();
                string shapeCommand = Console.ReadLine().ToLower();
                ProcessDrawCommand(canvas, shapeCommand);
            }
            else if (command == "clear")
            {
                canvas.Clear();
            }
        }
    }

    static string ShowDrawMenu()
    {
        Console.Clear();
        Console.WriteLine("Выберите фигуру для рисования:");
        Console.WriteLine("1. Point (точка)");
        Console.WriteLine("2. Line (линия)");
        Console.WriteLine("3. Rectangle (прямоугольник)");
        Console.Write("Введите номер фигуры: ");

        string choice = Console.ReadLine().ToLower();
        switch (choice)
        {
            case "1":
            case "point":
                return "point";
            case "2":
            case "line":
                return "line";
            case "3":
            case "rectangle":
                return "rect";
            default:
                Console.WriteLine("Неверный выбор, попробуйте снова.");
                return ShowDrawMenu(); // Повторный запрос выбора
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




