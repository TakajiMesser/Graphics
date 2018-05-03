using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.Behaviors.Leaves
{
    public class InlineLeafNode : Node
    {
        public Func<BehaviorContext, BehaviorStatus> Action { get; set; }

        public InlineLeafNode(Func<BehaviorContext, BehaviorStatus> action)
        {
            Action = action;
        }

        public override BehaviorStatus Tick(BehaviorContext context) => Action(context);

        public override void Reset() { }
    }
}
