using OpenTK;
using SavoryPhysicsCore.Bodies;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Utilities;
using System;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

namespace SampleGameProject.Behaviors.Enemy
{
    public class TurnTowardsNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.Entity is IActor actor)
            {
                if (((RigidBody3D)context.Body).LinearVelocity.IsSignificant())
                {
                    float turnAngle = (float)Math.Atan2(((RigidBody3D)context.Body).LinearVelocity.Y, ((RigidBody3D)context.Body).LinearVelocity.X);

                    actor.Rotation = new Quaternion(0.0f, 0.0f, turnAngle);
                    context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
