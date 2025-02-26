using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Создаем холст с размером 50x20
            Canvas canvas = new Canvas(50, 20);

            // Создаем меню и начинаем работу с ним
            Menu menu = new Menu(canvas);
            menu.ShowMenu();  // Показываем меню для взаимодействия с пользователем
        }
    }
}
