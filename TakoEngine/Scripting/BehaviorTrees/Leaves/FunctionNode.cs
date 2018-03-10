using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public class FunctionNode<T> : Node
    {
        [DataMember]
        public Func<T, BehaviorStatuses> Behavior { get; internal set; }

        [DataMember]
        public Dictionary<string, Type> Parameters { get; internal set; } 

        public FunctionNode() { }

        public override void Tick(BehaviorContext context)
        {
            if (!Status.IsComplete())
            {
                /*foreach (var kvp in Parameters)
                {
                    if (context.ContainsVariable(kvp.Key))
                    {
                        context.GetVariable()
                    }
                }
                Status = Behavior.Invoke(context);*/
            }
        }
    }
}
