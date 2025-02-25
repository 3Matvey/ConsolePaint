namespace ConsolePaint
{
    public abstract class Shape
    {
        public int Id { get; private set; }
        protected static int idCounter = 0;
        protected char Symbol;

        public Shape(char symbol)
        {
            Id = idCounter++;
            Symbol = symbol;
        }

        // Метод для рисования фигуры на холсте
        public abstract void Draw(Canvas canvas);

        // Метод для перемещения фигуры
        public abstract void Move(int dx, int dy);

        // Метод для заливки фигуры
        public abstract void Fill(Canvas canvas, char fillSymbol);
    }
}
