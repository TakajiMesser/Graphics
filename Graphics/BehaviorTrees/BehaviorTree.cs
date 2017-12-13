using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.BehaviorTrees
{
    public class BehaviorTree : INode
    {
        public BehaviorStatuses Status { get; private set; }
        public INode RootNode { get; set; }

        public void Tick()
        {
            if (!Status.IsComplete())
            {
                RootNode.Tick();
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
