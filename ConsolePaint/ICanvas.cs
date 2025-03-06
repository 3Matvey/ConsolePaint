namespace ConsolePaint
{
    public interface ICanvas
    {
        int Height { get; }
        int Width { get; }
        List<Shape> Shapes { get; }

        void DrawFrame();
        void Draw(Shape shape);
        void Fill(Shape shape);
        void Clear();
        void EraseShape(Shape shape);
        void AddShape(Shape shape);
        void RemoveShape(Shape shape);
        void RedrawAllShapes();
        void SetPixel(int x, int y, char symbol, ConsoleColor color);
        Pixel GetPixel(int x, int y);
    }
}
