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
        public Predicate<Dictionary<string, object>> Condition { get; set; }

        public ConditionNode(Predicate<Dictionary<string, object>> condition)
        {
            Condition = condition;
            Behavior = (v) => Condition.Invoke(v) ? BehaviorStatuses.Success : BehaviorStatuses.Failure;
        }
    }
}
