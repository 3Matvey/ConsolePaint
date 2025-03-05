namespace ConsolePaint.Commands
{
    public interface IUndoableAction
    {
        /// <summary>
        /// Выполнение действия (например, добавление фигуры).
        /// </summary>
        void Execute();

        /// <summary>
        /// Отмена действия (например, удаление ранее добавленной фигуры).
        /// </summary>
        void Undo();
    }
}