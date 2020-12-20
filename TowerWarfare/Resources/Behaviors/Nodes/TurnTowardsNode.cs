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

namespace TowerWarfare.Resources.Behaviors.Nodes
{
    public class TurnTowardsNode : Node
    {
        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.GetEntity() is IActor actor)
            {
                var body = context.GetComponent<IBody>() as RigidBody;

                if (body.LinearVelocity.IsSignificant())
                {
                    float turnAngle = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

                    actor.Rotation = new Quaternion(0.0f, 0.0f, turnAngle);
                    context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
