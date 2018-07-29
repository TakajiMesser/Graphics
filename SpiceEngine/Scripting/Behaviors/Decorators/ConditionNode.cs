using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Scripting.Behaviors.Decorators
{
    public abstract class ConditionNode : DecoratorNode
    {
        public ConditionNode(Node child) : base(child) { }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (Condition(context))
            {
                return Child.Tick(context);
            }
            else
            {
                return BehaviorStatus.Failure;
            }
        }

        public abstract bool Condition(BehaviorContext context);
    }
}
