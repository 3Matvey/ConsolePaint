using System;
using ConsolePaint;

namespace ConsolePaint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Задаём размеры холста: например, 50 столбцов и 20 строк (без учёта рамки)
            //Terminal terminal = new Terminal(70, 20);
            Terminal terminal = new Terminal();

            terminal.Run();
        }
    }
}
