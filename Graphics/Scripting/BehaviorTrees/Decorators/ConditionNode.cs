using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class ConditionNode : DecoratorNode
    {
        public delegate bool Condition(Dictionary<string, object> variablesByName);

        private Condition _condition;

        public ConditionNode(INode node, Condition condition) : base(node)
        {
            _condition = condition;
        }

        public override void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                Status = _condition.Invoke(variablesByName)
                    ? BehaviorStatuses.Success
                    : BehaviorStatuses.Failure;
            }
        }
    }
}
