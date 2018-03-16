using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class RepeaterNode : DecoratorNode
    {
        public RepeaterNode(Node node) : base(node) { }

        public override void Tick(BehaviorContext context)
        {
            Child.Tick(context);

            Status = Child.Status;

            if (Child.Status.IsComplete())
            {
                Child.Reset(true);
            }
        }
    }
}
