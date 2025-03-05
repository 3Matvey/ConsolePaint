namespace ConsolePaint.Shapes
{
    public class Point : Shape
    {
        private int x, y;

        public Point(int x, int y, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x = x;
            this.y = y;
            CalculatePixels();  // Изначально рассчитываем пиксели
        }

        // Метод для вычисления пикселей точки
        protected override void CalculatePixels()
        {
            // Очищаем старые пиксели
            OuterPixels.Clear();
            InnerPixels.Clear();  // Для точки внутренних пикселей нет

            // Добавляем пиксель для точки
            OuterPixels.Add(new Pixel(x, y, Symbol, Color));
        }
    }
}
