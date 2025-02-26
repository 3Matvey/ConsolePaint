using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public static class ShapeFactory
    {
        public static Shape CreateLine(int x1, int y1, int x2, int y2, char symbol = '*', ConsoleColor color = ConsoleColor.White)
        {
            return new Line(x1, y1, x2, y2, symbol, color);
        }

        public static Shape CreatePoint(int x, int y, char symbol = '*', ConsoleColor color = ConsoleColor.White)
        {
            return new Point(x, y, symbol, color);
        }

        public static Shape CreateRectangle(int x1, int y1, int x2, int y2, char symbol = '#', ConsoleColor color = ConsoleColor.White)
        {
            return new Rectangle(x1, y1, x2, y2, symbol, color);
        }
    }
}
