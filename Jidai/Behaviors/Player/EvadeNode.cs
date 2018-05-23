using TakoEngine.Scripting.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using TakoEngine.Entities;
using TakoEngine.Utilities;
using System.Runtime.Serialization;
using TakoEngine.Helpers;

namespace Jidai.Behaviors.Player
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
            var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

            if (context.ContainsVariable("coverDirection") && nEvadeTicks == 0 && context.InputState.IsPressed(context.InputMapping.Evade))
            {
                var evadeTranslation = GeometryHelper.GetHeldTranslation(context.Camera, EvadeSpeed, context.InputState, context.InputMapping);

                if (evadeTranslation != Vector3.Zero)
                {
                    // Find angle between possible evade and cover direction
                    var coverDirection = context.GetVariable<Vector2>("coverDirection");
                    float angle = coverDirection.AngleBetween(evadeTranslation.Xy);

                    // If this angle is greater than or equal to 90 degrees, let the evade take place
                    if (angle < (float)Math.PI / 2.0f)
                    {
                        return BehaviorStatus.Failure;
                    }
                    else
                    {
                        context.RemoveVariable("coverDirection");
                    }
                }
            }

            if (nEvadeTicks == 0 && context.InputState.IsPressed(context.InputMapping.Evade))
            {
                var evadeTranslation = GeometryHelper.GetHeldTranslation(context.Camera, EvadeSpeed, context.InputState, context.InputMapping);

                if (evadeTranslation != Vector3.Zero)
                {
                    nEvadeTicks++;

                    var angle = (float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X);

                    context.Actor.Rotation = new Quaternion(0.0f, 0.0f, (float)Math.Sin(angle / 2), (float)Math.Cos(angle / 2));
                    context.EulerRotation = new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), context.EulerRotation.Y, context.EulerRotation.Z);
                    context.Translation = evadeTranslation;

                    context.SetVariable("evadeTranslation", evadeTranslation);
                    context.SetVariable("nEvadeTicks", nEvadeTicks);

                    return BehaviorStatus.Success;
                }
            }

            else if (nEvadeTicks > 0 && nEvadeTicks <= TickCount)
            {
                var evadeTranslation = context.GetVariable<Vector3>("evadeTranslation");

                context.Actor.Rotation = Quaternion.FromAxisAngle(Vector3.Cross(evadeTranslation.Normalized(), -Vector3.UnitZ), 2.0f * (float)Math.PI / TickCount * nEvadeTicks);
                context.Actor.Rotation *= new Quaternion((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), 0.0f, 0.0f);
                context.EulerRotation = new Vector3(context.EulerRotation.X, 2.0f * (float)Math.PI / TickCount * nEvadeTicks, context.EulerRotation.Z);
                nEvadeTicks++;
                context.SetVariable("nEvadeTicks", nEvadeTicks);
                context.Translation = evadeTranslation;

                return BehaviorStatus.Success;
            }
            else if (nEvadeTicks > TickCount)
            {
                nEvadeTicks = 0;
                context.SetVariable("nEvadeTicks", nEvadeTicks);
                context.Actor.Rotation = Quaternion.Identity;
                context.EulerRotation = new Vector3(context.EulerRotation.X, 0.0f, context.EulerRotation.Z);

                return BehaviorStatus.Failure;
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
