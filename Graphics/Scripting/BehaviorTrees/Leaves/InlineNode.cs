using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Leaves
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
