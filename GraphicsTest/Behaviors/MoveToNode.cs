using Graphics.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors
{
    [DataContract]
    public class MoveToNode : LeafNode
    {
        [DataMember]
        public Vector3 Destination { get; private set; }

        [DataMember]
        public float Speed { get; private set; }

        public MoveToNode(Vector3 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var difference = Destination - context.Position;

            if (difference == Vector3.Zero)
            {
                return BehaviorStatuses.Success;
            }
            else if (difference.Length < Speed)
            {
                context.Translation = difference;
            }
            else
            {
                context.Translation = difference.Normalized() * Speed;
            }

            return BehaviorStatuses.Running;
        }
    }
}
