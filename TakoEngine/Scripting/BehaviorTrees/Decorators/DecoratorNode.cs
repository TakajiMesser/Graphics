using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public abstract class DecoratorNode : Node
    {
        [DataMember]
        public Node Child { get; private set; }

        public DecoratorNode(Node node) => Child = node;

        public override void Reset(bool recursive = false)
        {
            base.Reset();
            Child.Reset();
        }
    }
}
