namespace SpiceEngine.Scripting.Behaviors.Decorators
{
    public abstract class DecoratorNode : Node
    {
        public Node Child { get; private set; }

        public DecoratorNode(Node child) => Child = child;

        public override void Reset()
        {
            Child.Reset();
        }
    }
}
