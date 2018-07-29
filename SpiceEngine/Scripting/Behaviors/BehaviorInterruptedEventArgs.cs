using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Scripting.Behaviors
{
    public class BehaviorInterruptedEventArgs : EventArgs
    {
        public Node NewRootNode { get; private set; }

        public BehaviorInterruptedEventArgs(Node newRoot)
        {
            NewRootNode = newRoot;
        }
    }
}
