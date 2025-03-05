using ConsolePaint.Shapes;
using ConsolePaint;

namespace ConsolePaint.Commands
{
    public class AddShapeAction : IUndoableAction
    {
        private readonly Canvas canvas;
        private readonly Shape shape;

        public AddShapeAction(Canvas canvas, Shape shape)
        {
            this.canvas = canvas;
            this.shape = shape;
        }

        public void Execute()
        {
            canvas.AddShape(shape);
            canvas.RedrawAllShapes();
        }

        public void Undo()
        {
            canvas.RemoveShape(shape);
            canvas.RedrawAllShapes();
        }
    }
}
