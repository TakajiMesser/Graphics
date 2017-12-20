using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
{
    public class LeafNode : INode
    {
        public BehaviorStatuses Status { get; private set; }
        public delegate BehaviorStatuses Run();

        private Run _run;

        public LeafNode(Run run)
        {
            _run = run;
        }

        public void Tick(Dictionary<string, object> variablesByName)
        {
            Status = _run.Invoke();
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
        }
    }
}
