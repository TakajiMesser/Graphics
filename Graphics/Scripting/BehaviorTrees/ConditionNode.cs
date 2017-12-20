using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
{
    public class ConditionNode : INode
    {
        public BehaviorStatuses Status { get; private set; }

        public void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {

            }
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
        }
    }
}
