namespace ConsolePaint.Shapes
{
    public class Rectangle : Shape
    {
        private int x1, y1, x2, y2;

        public Rectangle() : base() { }

        public Rectangle(int x1, int y1, int x2, int y2, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            CalculatePixels();          }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear();

            // границы прямоугольника
            for (int x = x1; x <= x2; x++)
            {
                OuterPixels.Add(new Pixel(x, y1, Symbol, Color)); 
                OuterPixels.Add(new Pixel(x, y2, Symbol, Color)); 
            }

            for (int y = y1; y <= y2; y++)
            {
                OuterPixels.Add(new Pixel(x1, y, Symbol, Color)); 
                OuterPixels.Add(new Pixel(x2, y, Symbol, Color)); 
            }

            // внутри
            for (int x = x1 + 1; x < x2; x++)
            {
                for (int y = y1 + 1; y < y2; y++)
                {
                    InnerPixels.Add(new Pixel(x, y, ' ', Color)); 
                }
            }
        }
    }
}
