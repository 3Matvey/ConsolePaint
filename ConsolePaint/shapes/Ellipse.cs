using System.Drawing;

namespace ConsolePaint.Shapes
{
    public class Ellipse : Shape
    {
        private int centerX, centerY, radiusX, radiusY;
        
        public Ellipse() : base() { }

        public Ellipse(int centerX, int centerY, int radiusX, int radiusY, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.radiusX = radiusX;
            this.radiusY = radiusY;
            CalculatePixels();
        }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear();

            for (int y = centerY - radiusY; y <= centerY + radiusY; y++)
            {
                for (int x = centerX - radiusX; x <= centerX + radiusX; x++)
                {
                    // Эллипс в уравнении (x - centerX)^2 / radiusX^2 + (y - centerY)^2 / radiusY^2 = 1
                    double distance = Math.Pow((x - centerX) / (double)radiusX, 2) + Math.Pow((y - centerY) / (double)radiusY, 2);
                    if (distance <= 1)
                    {
                        if (Math.Abs(distance - 1) < 0.05)
                            OuterPixels.Add(new Pixel(x, y, Symbol, Color));  
                        else
                            InnerPixels.Add(new Pixel(x, y, ' ', Color));                     }
                }
            }
        }
    }
}

