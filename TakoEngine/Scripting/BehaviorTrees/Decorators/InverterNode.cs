using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node node) : base(node) { }

        public override void Tick(BehaviorContext context)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                Child.Tick(context);

                if (Child.Status == BehaviorStatuses.Success)
                {
                    Status = BehaviorStatuses.Failure;
                }
                else if (Child.Status == BehaviorStatuses.Failure)
                {
                    Status = BehaviorStatuses.Success;
                }
            }
        }
    }
}
