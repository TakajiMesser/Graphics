using System;

namespace SpiceEngineCore.Commands
{
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(ICommand command) => Command = command;

        public ICommand Command { get; }
    }
}
