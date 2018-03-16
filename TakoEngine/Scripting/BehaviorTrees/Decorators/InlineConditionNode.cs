using System;
using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class InlineConditionNode : ConditionNode
    {
        [DataMember]
        public Predicate<BehaviorContext> Predicate { get; set; }

        public InlineConditionNode(Predicate<BehaviorContext> predicate, Node node) : base(node)
        {
            Predicate = predicate;
        }

        public override bool Condition(BehaviorContext context) => Predicate(context);
    }
}
