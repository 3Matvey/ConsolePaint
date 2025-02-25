namespace ConsolePaint.Shapes
{
    class Line(int x1, int y1, int x2, int y2, char symbol) : Shape
    {
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
}
