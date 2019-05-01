using System;
using System.Windows.Input;

namespace SauceEditor.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecuteAction;

        public RelayCommand(Action<object> executeAction) : this(executeAction, null) { }
        public RelayCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecuteAction = canExecuteAction;
        }

        public void Execute(object parameter) => _executeAction(parameter);

        public bool CanExecute(object parameter) => _canExecuteAction?.Invoke(parameter) ?? true;

        /*public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }*/

        public event EventHandler CanExecuteChanged;
        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        //public void InvokeCanExecuteChanged(object parameter) => CanExecuteChanged?.Invoke(this, new EventArgs(parameter) { a });

        /*public void InvokeCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
            //CommandManager.RequerySuggested += CanExecuteChanged;
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }*/
    }
}