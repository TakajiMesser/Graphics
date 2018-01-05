using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees.Leaves
{
    [DataContract]
    public class NavigateNode : LeafNode
    {
        [DataMember]
        public Vector3 Destination { get; private set; }

        [DataMember]
        public float Speed { get; private set; }

        public NavigateNode(Vector3 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
            Behavior = (v) =>
            {
                var difference = Destination - v.Position;

                if (difference == Vector3.Zero)
                {
                    return BehaviorStatuses.Success;
                }
                else if (difference.Length < Speed)
                {
                    v.Translation = difference;
                }
                else
                {
                    v.Translation = difference.Normalized() * Speed;
                }

                return BehaviorStatuses.Running;
            };
        }
    }
}
