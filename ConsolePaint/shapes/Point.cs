namespace ConsolePaint.Shapes
{
    class Point(int x, int y, char symbol) : Shape
    {
        public override void Draw(Canvas canvas)
        {
            canvas.SetPixel(x, y, symbol);
        }
    }
}
