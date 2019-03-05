using System.Collections.Generic;

namespace SauceEditor.ViewModels.Commands
{
    public class CommandStack
    {
        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        private Stack<ICommand> _undoStack = new Stack<ICommand>();
        private Stack<ICommand> _redoStack = new Stack<ICommand>();

        public CommandStack() { }

        public void Push(ICommand command)
        {
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        public void Undo()
        {
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }

        public void Redo()
        {
            var command = _redoStack.Pop();
            command.Do();
            _undoStack.Push(command);
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }
}
