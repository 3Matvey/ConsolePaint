namespace ConsolePaint.Commands
{
    public class MoveShapeAction(ICanvas canvas, Shape shape, int dx, int dy) : IUndoableAction
    {
        public void Execute()
        {
            canvas.EraseShape(shape);
            shape.Move(dx, dy);
            canvas.RedrawAllShapes();
        }

        public void Undo()
        {
            canvas.EraseShape(shape);
            shape.Move(-dx, -dy);
            canvas.RedrawAllShapes();
        }
    }
}
