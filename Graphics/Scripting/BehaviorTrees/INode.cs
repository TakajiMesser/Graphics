using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
{
    public interface INode
    {
        BehaviorStatuses Status { get; }
        void Tick(Dictionary<string, object> variablesByName);
        void Reset();
    }
}
