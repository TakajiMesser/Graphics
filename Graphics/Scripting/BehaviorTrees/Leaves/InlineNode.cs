using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public class InlineNode : LeafNode
    {
        [DataMember]
        public Func<BehaviorContext, BehaviorStatuses> Function { get; set; }

        public InlineNode(Func<BehaviorContext, BehaviorStatuses> func)
        {
            Function = func;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context) => Function(context);
    }
}
