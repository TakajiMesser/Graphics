using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceEditor.Commands
{
    public class Command
    {
        public object Source { get; private set; }
        public Action Action { get; private set; }

        public Command(object source, Action action)
        {
            Source = source;
            Action = action;
        }
    }

    public class Command<T>
    {
        public object Source { get; private set; }
        public T Parameter { get; private set; }
        public Action<T> Action { get; private set; }

        public Command(object source, T parameter, Action<T> action)
        {
            Source = source;
            Parameter = parameter;
            Action = action;
        }
    }
}
