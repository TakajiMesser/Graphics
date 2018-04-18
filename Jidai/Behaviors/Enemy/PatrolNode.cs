using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Scripting.Properties;

namespace Jidai.Behaviors.Enemy
{
    public class PatrolNode : Node
    {
        public Vector3 Destination { get; private set; }
        public float Speed { get; private set; }

        public PatrolNode(Vector3 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var difference = Destination - context.Actor.Position;

            if (difference == Vector3.Zero)
            {
                return BehaviorStatus.Success;
            }
            else if (difference.Length < Speed)
            {
                context.Translation = difference;
            }
            else
            {
                context.Translation = difference.Normalized() * Speed;
            }

            if (context.Translation != Vector3.Zero)
            {
                float turnAngle = (float)Math.Atan2(context.Translation.Y, context.Translation.X);

                context.Actor.Rotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.EulerRotation = new Vector3(turnAngle, context.EulerRotation.Y, context.EulerRotation.Z);
            }

            return BehaviorStatus.Running;
        }
    }
}
