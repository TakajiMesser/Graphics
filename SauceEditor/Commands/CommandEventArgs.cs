using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceEditor.Commands
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
