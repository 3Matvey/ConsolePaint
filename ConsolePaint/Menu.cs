using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Menu
    {
        private Canvas canvas;
        private List<Shape> shapes;  // Список созданных фигур

        public Menu(Canvas canvas)
        {
            this.canvas = canvas;
            this.shapes = new List<Shape>();
        }

        // Метод для отображения и обработки команд
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите команду:");
                Console.WriteLine("1. Нарисовать фигуру (draw)");
                Console.WriteLine("2. Очистить холст (clear)");
                Console.WriteLine("3. Выйти (exit)");

                string choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "1":
                        HandleDrawCommand();
                        break;
                    case "2":
                        canvas.Clear();
                        shapes.Clear();  // Очищаем список фигур
                        break;
                    case "3":
                        return;  // Выход из программы
                    default:
                        Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                        break;
                }
            }
        }

        // Метод для обработки команды рисования
        private void HandleDrawCommand()
        {
            Console.Clear();
            Console.WriteLine("Выберите фигуру для рисования:");
            Console.WriteLine("1. Линия");
            Console.WriteLine("2. Точка");
            Console.WriteLine("3. Прямоугольник");

            string shapeChoice = Console.ReadLine().ToLower();

            Shape shape = null;
            switch (shapeChoice)
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
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    return;
            }

            shapes.Add(shape);  // Добавляем фигуру в список
            canvas.Draw(shape);  // Рисуем фигуру на холсте
        }

        // Метод для создания линии
        private Shape CreateLine()
        {
            Console.WriteLine("Введите координаты начала линии (x1, y1):");
            int x1 = int.Parse(Console.ReadLine());
            int y1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите координаты конца линии (x2, y2):");
            int x2 = int.Parse(Console.ReadLine());
            int y2 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите символ для линии (по умолчанию '*'):");
            char symbol = Console.ReadLine().Length > 0 ? Console.ReadLine()[0] : '*';

            Console.WriteLine("Введите цвет для линии (по умолчанию White):");
            string colorInput = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(colorInput, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateLine(x1, y1, x2, y2, symbol, color);
        }

        // Метод для создания точки
        private Shape CreatePoint()
        {
            Console.WriteLine("Введите координаты точки (x, y):");
            int x = int.Parse(Console.ReadLine());
            int y = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите символ для точки (по умолчанию '*'):");
            char symbol = Console.ReadLine().Length > 0 ? Console.ReadLine()[0] : '*';

            Console.WriteLine("Введите цвет для точки (по умолчанию White):");
            string colorInput = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(colorInput, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreatePoint(x, y, symbol, color);
        }

        // Метод для создания прямоугольника
        private Shape CreateRectangle()
        {
            Console.WriteLine("Введите координаты верхнего левого угла прямоугольника (x1, y1):");
            int x1 = int.Parse(Console.ReadLine());
            int y1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите координаты нижнего правого угла прямоугольника (x2, y2):");
            int x2 = int.Parse(Console.ReadLine());
            int y2 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите символ для прямоугольника (по умолчанию '#'):");
            char symbol = Console.ReadLine().Length > 0 ? Console.ReadLine()[0] : '#';

            Console.WriteLine("Введите цвет для прямоугольника (по умолчанию White):");
            string colorInput = Console.ReadLine();
            ConsoleColor color = Enum.TryParse(colorInput, true, out color) ? color : ConsoleColor.White;

            return ShapeFactory.CreateRectangle(x1, y1, x2, y2, symbol, color);
        }
    }
}
