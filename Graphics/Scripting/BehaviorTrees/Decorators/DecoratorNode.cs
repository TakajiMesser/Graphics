using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public abstract class DecoratorNode : Node
    {
        [DataMember]
        public Node Child { get; private set; }

        public DecoratorNode(Node node) => Child = node;

        public override void Reset(bool recursive = false)
        {
            base.Reset();
            Child.Reset();
        }
    }
}
