using System;

namespace SauceEditor.ViewModels.Commands
{
    public class CommandEventArgs : EventArgs
    {
        public ICommand Command { get; private set; }

        public CommandEventArgs(ICommand command)
        {
            Command = command;
        }
    }
}
