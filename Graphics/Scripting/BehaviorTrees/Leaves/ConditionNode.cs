using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public class ConditionNode : LeafNode
    {
        [DataMember]
        public Predicate<BehaviorContext> Condition { get; set; }

        public ConditionNode(Predicate<BehaviorContext> condition)
        {
            Condition = condition;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            return Condition.Invoke(context) ? BehaviorStatuses.Success : BehaviorStatuses.Failure;
        }
    }
}
