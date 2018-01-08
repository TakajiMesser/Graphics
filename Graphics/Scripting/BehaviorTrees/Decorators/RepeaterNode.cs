using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class RepeaterNode : DecoratorNode
    {
        public RepeaterNode(Node node) : base(node) { }

        public override void Tick(BehaviorContext context)
        {
            Child.Tick(context);

            Status = Child.Status;

            if (Child.Status.IsComplete())
            {
                Child.Reset(true);
            }
        }
    }
}
