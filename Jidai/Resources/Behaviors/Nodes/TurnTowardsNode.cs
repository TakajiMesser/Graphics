using OpenTK;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Utilities;
using System;

namespace Jidai.Resources.Behaviors.Nodes
{
    public class TurnTowardsNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (((RigidBody3D)context.Body).LinearVelocity.IsSignificant())
            {
                float turnAngle = (float)Math.Atan2(((RigidBody3D)context.Body).LinearVelocity.Y, ((RigidBody3D)context.Body).LinearVelocity.X);

                context.Actor.Rotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                context.EulerRotation = new Vector3(turnAngle, context.EulerRotation.Y, context.EulerRotation.Z);
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
