using System;
using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public class InlineLeafNode : LeafNode
    {
        [DataMember]
        public Func<BehaviorContext, BehaviorStatuses> Action { get; set; }

        public InlineLeafNode(Func<BehaviorContext, BehaviorStatuses> action)
        {
            Action = action;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context) => Action(context);
    }
}
