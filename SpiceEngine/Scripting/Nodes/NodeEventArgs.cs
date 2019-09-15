using System;

namespace SpiceEngine.Scripting.Nodes
{
    public class NodeEventArgs : EventArgs
    {
        public Node Node { get; private set; }

        public NodeEventArgs(Node node) => Node = node;
    }
}
