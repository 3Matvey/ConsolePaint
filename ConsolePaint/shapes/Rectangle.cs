namespace ConsolePaint.Shapes
{
    public class Rectangle : Shape
    {
        private readonly int x1, y1, x2, y2;

        public Rectangle() : base() { }

        public Rectangle(int x1, int y1, int x2, int y2, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            CalculatePixels();          
        }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear();

            int left = Math.Min(x1, x2);
            int right = Math.Max(x1, x2);
            int top = Math.Min(y1, y2);
            int bottom = Math.Max(y1, y2);

            for (int x = left; x <= right; x++)
            {
                OuterPixels.Add(new Pixel(x, top, Symbol, Color));
                OuterPixels.Add(new Pixel(x, bottom, Symbol, Color));
            }

            for (int y = top; y <= bottom; y++)
            {
                OuterPixels.Add(new Pixel(left, y, Symbol, Color));
                OuterPixels.Add(new Pixel(right, y, Symbol, Color));
            }

            for (int x = left + 1; x < right; x++)
            {
                for (int y = top + 1; y < bottom; y++)
                {
                    InnerPixels.Add(new Pixel(x, y, ' ', Color));
                }
            }
        }

    }
}
