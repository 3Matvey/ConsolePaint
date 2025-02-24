namespace ConsolePaint.shapes
{

    class Point : Shape
    {
        private int x, y;
        private char symbol;

        public Point(int x, int y, char symbol)
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
        }

        public override void Draw(Canvas canvas)
        {
            canvas.SetPixel(x, y, symbol);
        }
    }

    class Line : Shape
    {
        private int x1, y1, x2, y2;
        private char symbol;

        public Line(int x1, int y1, int x2, int y2, char symbol)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.symbol = symbol;
        }

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

    class Rectangle : Shape
    {
        private int x1, y1, x2, y2;
        private char symbol;

        public Rectangle(int x1, int y1, int x2, int y2, char symbol)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.symbol = symbol;
        }

        public override void Draw(Canvas canvas)
        {
            // Рисуем верхнюю и нижнюю горизонтальные линии
            for (int x = x1; x <= x2; x++)
            {
                canvas.SetPixel(x, y1, symbol);
                canvas.SetPixel(x, y2, symbol);
            }

            // Рисуем боковые вертикальные линии
            for (int y = y1; y <= y2; y++)
            {
                canvas.SetPixel(x1, y, symbol);
                canvas.SetPixel(x2, y, symbol);
            }
        }
    }
}
