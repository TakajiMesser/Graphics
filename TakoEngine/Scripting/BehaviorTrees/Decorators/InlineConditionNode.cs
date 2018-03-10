using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
