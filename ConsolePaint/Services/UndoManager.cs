using System.Collections.Generic;
using ConsolePaint.Commands;

namespace ConsolePaint.Services
{
    public class UndoManager
    {
        private readonly Stack<IUndoableAction> undoStack = new();
        private readonly Stack<IUndoableAction> redoStack = new();

        /// <summary>
        /// Выполняет действие, сохраняет его в стек undo и очищает стек redo.
        /// </summary>
        public void ExecuteAction(IUndoableAction action)
        {
            action.Execute();
            undoStack.Push(action);
            redoStack.Clear();
        }

        /// <summary>
        /// Отменяет последнее действие.
        /// </summary>
        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                IUndoableAction action = undoStack.Pop();
                action.Undo();
                redoStack.Push(action);
            }
        }

        /// <summary>
        /// Повторяет последнее отменённое действие.
        /// </summary>
        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                IUndoableAction action = redoStack.Pop();
                action.Execute();
                undoStack.Push(action);
            }
        }
    }
}
