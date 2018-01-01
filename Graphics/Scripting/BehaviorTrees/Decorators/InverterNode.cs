using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node node) : base(node) { }

        public override void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                Child.Tick(variablesByName);

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
