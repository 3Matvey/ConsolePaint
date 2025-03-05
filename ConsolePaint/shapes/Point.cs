using System.Drawing;

namespace ConsolePaint.Shapes
{
    public class Point : Shape
    {
        private int x, y;

        public Point() : base() { }
        public Point(int x, int y, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x = x;
            this.y = y;
            CalculatePixels();  
        }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear(); 

            OuterPixels.Add(new Pixel(x, y, Symbol, Color));
        }
    }
}
