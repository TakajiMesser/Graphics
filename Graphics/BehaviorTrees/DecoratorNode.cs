using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.BehaviorTrees
{
    public class DecoratorNode : INode
    {
        public BehaviorStatuses Status { get; private set; }

        public void Tick()
        {

        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
        }
    }
}
