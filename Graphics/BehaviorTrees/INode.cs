using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.BehaviorTrees
{
    public interface INode
    {
        BehaviorStatuses Status { get; }
        void Tick();
        void Reset();
    }
}
