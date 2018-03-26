using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.Behaviors.Decorators
{
    public class InlineConditionNode : ConditionNode
    {
        public Predicate<BehaviorContext> Predicate { get; set; }

        public InlineConditionNode(Predicate<BehaviorContext> predicate, Node node) : base(node)
        {
            Predicate = predicate;
        }

        public override bool Condition(BehaviorContext context) => Predicate(context);
    }
}
