using System;

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
