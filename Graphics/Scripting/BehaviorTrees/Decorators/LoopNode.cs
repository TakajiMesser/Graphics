using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class LoopNode : DecoratorNode
    {
        [DataMember]
        public int Repetitions { get; private set; }

        [DataMember]
        public int Count { get; private set; }

        public LoopNode(INode node, int repetitions) : base(node)
        {
            if (repetitions <= 0)
            {
                throw new ArgumentOutOfRangeException("LoopNode repetitions must be positive");
            }

            Repetitions = repetitions;
        }

        public override void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                if (Count < Repetitions)
                {
                    if (Child.Status.IsComplete())
                    {
                        Child.Reset();
                    }

                    Child.Tick(variablesByName);

                    if (Child.Status == BehaviorStatuses.Success)
                    {
                        Count++;
                    }
                    else if (Child.Status == BehaviorStatuses.Failure)
                    {
                        Status = BehaviorStatuses.Failure;
                    }
                }
                else
                {
                    Status = BehaviorStatuses.Success;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            Count = 0;
        }
    }
}
