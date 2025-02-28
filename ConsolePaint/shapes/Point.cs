namespace ConsolePaint.Shapes
{
    public class Point : Shape
    {
        private int x, y;
        private char symbol;
        private ConsoleColor color;

        public Point(int x, int y, char symbol, ConsoleColor color)
            : base()
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            this.color = color;
            CalculatePixels();  // Изначально рассчитываем пиксели
        }

        // Метод для вычисления пикселей точки
        protected override void CalculatePixels()
        {
            // Очищаем старые пиксели
            OuterPixels.Clear();
            InnerPixels.Clear();  // Для точки внутренних пикселей нет

            // Добавляем пиксель для точки
            OuterPixels.Add(new Pixel(x, y, symbol, color));
        }
    }
}
