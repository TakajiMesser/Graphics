using UmamiScriptingCore.Behaviors.Nodes;
using System;

namespace UmamiScriptingCore.Behaviors
{
    public class BehaviorInterruptedEventArgs : EventArgs
    {
        public Node NewRootNode { get; private set; }

        public BehaviorInterruptedEventArgs(Node newRoot) => NewRootNode = newRoot;
    }
}
