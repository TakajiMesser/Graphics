using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public abstract class LeafNode : Node
    {
        public LeafNode() { }

        public override void Tick(BehaviorContext context)
        {
            if (!Status.IsComplete())
            {
                Status = Behavior(context);
            }
        }

        public abstract BehaviorStatuses Behavior(BehaviorContext context);
    }
}
