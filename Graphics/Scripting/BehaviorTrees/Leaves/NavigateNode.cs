using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Scripting.BehaviorTrees
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
        }

        public override void Tick(Dictionary<string, object> variablesByName)
        {
            if (!Status.IsComplete())
            {
                Status = BehaviorStatuses.Running;

                var currentPosition = (Vector3)variablesByName["Position"];
                var difference = Position - currentPosition;

                if (difference == Vector3.Zero)
                {
                    Status = BehaviorStatuses.Success;
                }
                else if (difference.Length < Speed)
                {
                    variablesByName["Translation"] = difference;
                }
                else
                {
                    variablesByName["Translation"] = difference.Normalized() * Speed;
                }
            }
        }
    }
}
