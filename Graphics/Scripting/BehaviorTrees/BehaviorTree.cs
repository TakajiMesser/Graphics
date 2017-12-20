using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
{
    public class BehaviorTree
    {
        public BehaviorStatuses Status { get; private set; }
        public Dictionary<string, object> VariablesByName { get; }
        public INode RootNode { get; set; }

        public void Tick()
        {
            if (!Status.IsComplete())
            {
                RootNode.Tick(VariablesByName);
                Status = RootNode.Status;
            }
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
            RootNode.Reset();
        }
    }
}
