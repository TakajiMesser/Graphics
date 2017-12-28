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
        public Vector3 Position { get; private set; }

        [DataMember]
        public float Speed { get; private set; }

        public NavigateNode(Vector3 position, float speed)
        {
            Position = position;
            Speed = speed;
            Behavior = (v) =>
            {
                var currentPosition = (Vector3)v["Position"];
                var difference = Position - currentPosition;

                if (difference == Vector3.Zero)
                {
                    return BehaviorStatuses.Success;
                }
                else if (difference.Length < Speed)
                {
                    v["Translation"] = difference;
                }
                else
                {
                    v["Translation"] = difference.Normalized() * Speed;
                }

                return BehaviorStatuses.Running;
            };
        }
    }
}
