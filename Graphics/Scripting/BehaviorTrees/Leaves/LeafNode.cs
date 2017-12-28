using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public class LeafNode : INode
    {
        public BehaviorStatuses Status { get; protected set; }

        [DataMember]
        public Func<Dictionary<string, object>, BehaviorStatuses> Behavior { get; internal set; }

        public LeafNode() { }

        public void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = Behavior.Invoke(variablesByName);
            }
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
        }
    }
}
