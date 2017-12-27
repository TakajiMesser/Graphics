using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
{
    [DataContract]
    public abstract class LeafNode : INode
    {
        public BehaviorStatuses Status { get; protected set; }

        public LeafNode() { }

        public abstract void Tick(Dictionary<string, object> variablesByName);

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
        }
    }
}
