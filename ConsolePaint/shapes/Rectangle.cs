namespace ConsolePaint.Shapes
{
    public class Rectangle : Shape
    {
        private int x1, y1, x2, y2;

        public Rectangle(int x1, int y1, int x2, int y2, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.symbol = symbol;
            this.color = color;
            CalculatePixels();  // Изначально рассчитываем пиксели
        }

        // Метод для вычисления пикселей прямоугольника
        protected override void CalculatePixels()
        {
            // Очищаем старые пиксели
            OuterPixels.Clear();
            InnerPixels.Clear();

            // Внешние пиксели (границы прямоугольника)
            for (int x = x1; x <= x2; x++)
            {
                OuterPixels.Add(new Pixel(x, y1, symbol, color)); // Верхняя граница
                OuterPixels.Add(new Pixel(x, y2, symbol, color)); // Нижняя граница
            }

            for (int y = y1; y <= y2; y++)
            {
                OuterPixels.Add(new Pixel(x1, y, symbol, color)); // Левая граница
                OuterPixels.Add(new Pixel(x2, y, symbol, color)); // Правая граница
            }

            // Внутренние пиксели (внутри прямоугольника)
            for (int x = x1 + 1; x < x2; x++)
            {
                for (int y = y1 + 1; y < y2; y++)
                {
                    InnerPixels.Add(new Pixel(x, y, ' ', color)); // Внутренние пиксели
                }
            }
        }
    }
}
