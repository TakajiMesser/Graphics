using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Utilities;
using System;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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

                    actor.Rotation = Quaternion.FromEulerAngles(0.0f, 0.0f, turnAngle);
                    context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
