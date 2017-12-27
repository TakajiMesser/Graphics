using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public abstract class DecoratorNode : INode
    {
        public BehaviorStatuses Status { get; protected set; }

        [DataMember]
        public INode Child { get; private set; }

        public DecoratorNode(INode node)
        {
            Child = node;
        }

        public abstract void Tick(Dictionary<string, object> variablesByName);

        public virtual void Reset()
        {
            Status = BehaviorStatuses.Dormant;
            Child.Reset();
        }
    }
}
