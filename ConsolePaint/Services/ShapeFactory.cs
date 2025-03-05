using ConsolePaint.Shapes;

namespace ConsolePaint.Services
{
    public static class ShapeFactory
    {
        public static Shape CreateLine(int x1, int y1, int x2, int y2, char symbol = '*', ConsoleColor color = ConsoleColor.White)
            => new Line(x1, y1, x2, y2, symbol, color);

        public static Shape CreatePoint(int x, int y, char symbol = '*', ConsoleColor color = ConsoleColor.White)
            => new Point(x, y, symbol, color);

        public static Shape CreateRectangle(int x1, int y1, int x2, int y2, char symbol = '#', ConsoleColor color = ConsoleColor.White)
            => new Rectangle(x1, y1, x2, y2, symbol, color);


        public static Shape CreateEllipse(int centerX, int centerY, int radiusX, int radiusY = 0, char symbol = '.', ConsoleColor color = ConsoleColor.White)
        {
            if (radiusY == 0) radiusY = radiusX;

            return new Ellipse(centerX, centerY, radiusX, radiusY, symbol, color);
        }

        public static Shape CreateTriangle(int x1, int y1, int x2, int y2, int x3, int y3, char symbol = '.', ConsoleColor color = ConsoleColor.White)
            => new Triangle(x1, y1, x2, y2, x3, y3, symbol, color);

    }
}
