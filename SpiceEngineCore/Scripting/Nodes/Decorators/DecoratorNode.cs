namespace SpiceEngineCore.Scripting.Nodes.Decorators
{
    public abstract class DecoratorNode : Node
    {
        public Node Child { get; private set; }

        public DecoratorNode(Node child)
        {
            Child = child;

            // Insert the script-node as a placeholder, to be overwritten upon compilation
            if (child is ScriptNode scriptNode)
            {
                scriptNode.Compiled += (s, args) => Child = args.Node;
            }
        }

        public override void Reset() => Child.Reset();
    }
}
