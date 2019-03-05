using System;

namespace SauceEditor.ViewModels.Commands
{
    public class Command : ICommand
    {
        public object Source { get; private set; }

        private Action _doAction;
        private Action _undoAction;

        public Command(object source, Action doAction, Action undoAction)
        {
            Source = source;
            _doAction = doAction;
            _undoAction = undoAction;
        }

        public void Do() => _doAction();
        public void Undo() => _undoAction();
    }

    public class Command<T>
    {
        public object Source { get; private set; }

        private T _parameter;
        private Action<T> _doAction;
        private Action<T> _undoAction;

        public Command(object source, T parameter, Action<T> doAction, Action<T> undoAction)
        {
            Source = source;
            _parameter = parameter;
            _doAction = doAction;
            _undoAction = undoAction;
        }

        public void Do() => _doAction(_parameter);
        public void Undo() => _undoAction(_parameter);
    }
}
