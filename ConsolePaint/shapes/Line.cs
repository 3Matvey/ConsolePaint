namespace ConsolePaint.Shapes
{
    public class Line : Shape
    {
        private int x1, y1, x2, y2;
  
        public Line() : base() { }
        public Line(int x1, int y1, int x2, int y2, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            CalculatePixels();  // Изначально рассчитываем пиксели
        }

        // Метод для вычисления пикселей линии
        protected override void CalculatePixels()
        {
            // Очищаем старые пиксели
            OuterPixels.Clear();
            InnerPixels.Clear(); // Для линии внутренних пикселей нет

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                OuterPixels.Add(new Pixel(x1, y1, Symbol, Color));  // Добавляем пиксель на контур линии

                if (x1 == x2 && y1 == y2) break;

                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }
    }
}
