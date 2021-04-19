using SavoryPhysicsCore;
using SpiceEngine.Helpers;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Utilities;
using System;
using TangyHIDCore;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

namespace SampleGameProject.Behaviors.Player
{
    public class EvadeNode : Node
    {
        public float EvadeSpeed { get; set; }
        public int TickCount { get; set; }

        public EvadeNode(float evadeSpeed, int tickCount)
        {
            EvadeSpeed = evadeSpeed;
            TickCount = tickCount;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.GetEntity() is IActor actor)
            {
                var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();
                var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

                if (context.ContainsVariable("coverDirection") && nEvadeTicks == 0 && inputProvider.IsPressed("Evade"))
                {
                    var evadeTranslation = GeometryHelper.GetHeldTranslation(context.ActiveCamera, EvadeSpeed, inputProvider);

                    if (evadeTranslation.IsSignificant())
                    {
                        // Find angle between possible evade and cover direction
                        var coverDirection = context.GetVariable<Vector2>("coverDirection");
                        float angle = coverDirection.AngleBetween(evadeTranslation.Xy);

                        // If this angle is greater than or equal to 90 degrees, let the evade take place
                        if (angle < MathExtensions.HALF_PI)
                        {
                            return BehaviorStatus.Failure;
                        }
                        else
                        {
                            context.RemoveVariable("coverDirection");
                        }
                    }
                }

                if (nEvadeTicks == 0 && inputProvider.IsPressed("Evade"))
                {
                    var evadeTranslation = GeometryHelper.GetHeldTranslation(context.ActiveCamera, EvadeSpeed, inputProvider);

                    if (evadeTranslation.IsSignificant())
                    {
                        nEvadeTicks++;

                        var angle = (float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X);

                        actor.Rotation = new Quaternion(0.0f, 0.0f, (float)Math.Sin(angle / 2), (float)Math.Cos(angle / 2));
                        context.EulerRotation = new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), context.EulerRotation.Y, context.EulerRotation.Z);

                        var body = context.GetComponent<IBody>();
                        body.ApplyVelocity(evadeTranslation);

                        context.SetVariable("evadeTranslation", evadeTranslation);
                        context.SetVariable("nEvadeTicks", nEvadeTicks);

                        return BehaviorStatus.Success;
                    }
                }

                else if (nEvadeTicks > 0 && nEvadeTicks <= TickCount)
                {
                    var evadeTranslation = context.GetVariable<Vector3>("evadeTranslation");

                    actor.Rotation = Quaternion.FromAxisAngle(Vector3.Cross(evadeTranslation.Normalized(), -Vector3.UnitZ), MathExtensions.TWO_PI / TickCount * nEvadeTicks);
                    actor.Rotation *= new Quaternion(0.0f, 0.0f, (float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X));
                    context.EulerRotation = new Vector3(context.EulerRotation.X, MathExtensions.TWO_PI / TickCount * nEvadeTicks, context.EulerRotation.Z);

                    nEvadeTicks++;
                    context.SetVariable("nEvadeTicks", nEvadeTicks);

                    var body = context.GetComponent<IBody>();
                    body.ApplyVelocity(evadeTranslation);

                    return BehaviorStatus.Success;
                }
                else if (nEvadeTicks > TickCount)
                {
                    nEvadeTicks = 0;
                    context.SetVariable("nEvadeTicks", nEvadeTicks);
                    actor.Rotation = Quaternion.Identity;
                    context.EulerRotation = new Vector3(context.EulerRotation.X, 0.0f, context.EulerRotation.Z);

                    return BehaviorStatus.Failure;
                }
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
