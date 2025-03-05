using ConsolePaint.Shapes;
using ConsolePaint;

namespace ConsolePaint.Commands
{
    public class RemoveShapeAction : IUndoableAction
    {
        private readonly Canvas canvas;
        private readonly Shape shape;

        public RemoveShapeAction(Canvas canvas, Shape shape)
        {
            this.canvas = canvas;
            this.shape = shape;
        }

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
