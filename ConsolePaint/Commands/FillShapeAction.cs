namespace ConsolePaint.Commands
{
    public class FillShapeAction : IUndoableAction
    {
        private readonly ICanvas canvas;
        private readonly Shape shape;
        private readonly char newSymbol;
        private readonly ConsoleColor newColor;
        private readonly List<(int index, char oldSymbol, ConsoleColor oldColor)> originalState;

        public FillShapeAction(ICanvas canvas, Shape shape, char newSymbol, ConsoleColor newColor)
        {
            this.canvas = canvas;
            this.shape = shape;
            this.newSymbol = newSymbol;
            this.newColor = newColor;
            originalState = [];

            // исходное состояние внутренних пикселей фигуры
            for (int i = 0; i < shape.InnerPixels.Count; i++)
            {
                var p = shape.InnerPixels[i];
                originalState.Add((i, p.Symbol, p.Color));
            }
        }

        public void Execute()
        {
            foreach (var pixel in shape.InnerPixels)
            {
                pixel.Symbol = newSymbol;
                pixel.Color = newColor;
            }
            canvas.Fill(shape);
        }

        public void Undo()
        {
            // сохраненное состояние внутренних пикселей
            foreach (var (index, oldSymbol, oldColor) in originalState)
            {
                if (index < shape.InnerPixels.Count)
                {
                    shape.InnerPixels[index].Symbol = oldSymbol;
                    shape.InnerPixels[index].Color = oldColor;
                }
            }
            canvas.Fill(shape);
        }
    }
}
