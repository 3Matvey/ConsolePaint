using ConsolePaint.Shapes;
using ConsolePaint;

namespace ConsolePaint.Commands
{
    public class RemoveShapeAction(ICanvas canvas, Shape shape) : IUndoableAction
    {
        public void Execute()
        {
            canvas.RemoveShape(shape);
            canvas.RedrawAllShapes();
        }

        public void Undo()
        {
            canvas.AddShape(shape);
            canvas.RedrawAllShapes();
        }
    }
}
