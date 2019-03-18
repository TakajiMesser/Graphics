using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.Properties;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Utilities;

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
            var difference = Destination - context.Position;

            if (!difference.IsSignificant())
            {
                return BehaviorStatus.Success;
            }
            else if (difference.Length < Speed)
            {
                ((RigidBody3D)context.Body).ApplyVelocity(difference);
            }
            else
            {
                ((RigidBody3D)context.Body).ApplyVelocity(difference.Normalized() * Speed);
            }

            if (((RigidBody3D)context.Body).LinearVelocity.IsSignificant())
            {
                float turnAngle = (float)Math.Atan2(((RigidBody3D)context.Body).LinearVelocity.Y, ((RigidBody3D)context.Body).LinearVelocity.X);

                context.Actor.Rotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.EulerRotation = new Vector3(turnAngle, context.EulerRotation.Y, context.EulerRotation.Z);
            }

            return BehaviorStatus.Running;
        }

        public override void Reset() { }
    }
}
