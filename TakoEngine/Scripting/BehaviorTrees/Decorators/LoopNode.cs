using System;
using System.Runtime.Serialization;

namespace TakoEngine.Scripting.BehaviorTrees.Decorators
{
    [DataContract]
    public class LoopNode : DecoratorNode
    {
        [DataMember]
        public int Repetitions { get; private set; }

        [DataMember]
        public int Count { get; private set; }

        public LoopNode(Node node, int repetitions) : base(node)
        {
            if (repetitions <= 0)
            {
                throw new ArgumentOutOfRangeException("LoopNode repetitions must be positive");
            }

            Repetitions = repetitions;
        }

        public override void Tick(BehaviorContext context)
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

                    Child.Tick(context);

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

        public override void Reset(bool recursive = false)
        {
            base.Reset();
            Count = 0;
        }
    }
}
