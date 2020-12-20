using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Utilities;
using System;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SampleGameProject.Behaviors.Enemy
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
            var difference = Destination - context.GetPosition();

            if (difference.IsSignificant())
            {
                var velocity = difference.Length < Speed
                    ? difference
                    : difference.Normalized() * Speed;

                var body = context.GetComponent<IBody>() as RigidBody;
                body.ApplyVelocity(velocity);

                if (context.GetEntity() is IActor actor && velocity.IsSignificant())
                {
                    float turnAngle = (float)Math.Atan2(velocity.Y, velocity.X);

                    actor.Rotation = new Quaternion(0.0f, 0.0f, turnAngle);
                    context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
                }

                return BehaviorStatus.Running;
            }
            else
            {
                return BehaviorStatus.Success;
            }
        }

        public override void Reset() { }
    }
}
