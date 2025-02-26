namespace ConsolePaint
{
    public class Pixel
    {
        public int X;
        public int Y;
        public char Symbol;
        public ConsoleColor Color;

        public Pixel(int x, int y, char symbol, ConsoleColor color)
        {
            X = x;
            Y = y;
            Symbol = symbol;
            Color = color;
        }
    }
}


