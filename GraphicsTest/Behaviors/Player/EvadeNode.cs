using TakoEngine.Scripting.BehaviorTrees.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using TakoEngine.GameObjects;
using TakoEngine.Utilities;
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Player
{
    [DataContract]
    public class EvadeNode : LeafNode
    {
        [DataMember]
        public float EvadeSpeed { get; set; }

        [DataMember]
        public int TickCount { get; set; }

        public EvadeNode(float evadeSpeed, int tickCount)
        {
            EvadeSpeed = evadeSpeed;
            TickCount = tickCount;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

            if (context.ContainsVariable("coverDirection") && nEvadeTicks == 0 && context.InputState.IsPressed(context.InputMapping.Evade))
            {
                var evadeTranslation = context.GetTranslation(EvadeSpeed);

                if (evadeTranslation != Vector3.Zero)
                {
                    // Find angle between possible evade and cover direction
                    var coverDirection = context.GetVariable<Vector2>("coverDirection");
                    float angle = coverDirection.AngleBetween(evadeTranslation.Xy);

                    // If this angle is greater than or equal to 90 degrees, let the evade take place
                    if (angle < (float)Math.PI / 2.0f)
                    {
                        return BehaviorStatuses.Failure;
                    }
                    else
                    {
                        context.RemoveVariable("coverDirection");
                    }
                }
            }

            if (nEvadeTicks == 0 && context.InputState.IsPressed(context.InputMapping.Evade))
            {
                var evadeTranslation = context.GetTranslation(EvadeSpeed);

                if (evadeTranslation != Vector3.Zero)
                {
                    nEvadeTicks++;

                    var angle = (float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X);

                    context.QRotation = new Quaternion(0.0f, 0.0f, (float)Math.Sin(angle / 2), (float)Math.Cos(angle / 2));
                    context.Rotation = new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), context.Rotation.Y, context.Rotation.Z);
                    //context.Scale = new Vector3(1.0f, 0.5f, 1.0f);
                    context.Translation = evadeTranslation;

                    context.SetVariable("evadeTranslation", evadeTranslation);
                    context.SetVariable("nEvadeTicks", nEvadeTicks);

                    return BehaviorStatuses.Success;
                }
            }

            else if (nEvadeTicks > 0 && nEvadeTicks <= TickCount)
            {
                var evadeTranslation = context.GetVariable<Vector3>("evadeTranslation");

                context.QRotation = Quaternion.FromAxisAngle(Vector3.Cross(evadeTranslation.Normalized(), -Vector3.UnitZ), 2.0f * (float)Math.PI / TickCount * nEvadeTicks);
                context.QRotation *= new Quaternion((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), 0.0f, 0.0f);
                context.Rotation = new Vector3(context.Rotation.X, 2.0f * (float)Math.PI / TickCount * nEvadeTicks, context.Rotation.Z);
                nEvadeTicks++;
                context.SetVariable("nEvadeTicks", nEvadeTicks);
                context.Translation = evadeTranslation;

                return BehaviorStatuses.Success;
            }
            else if (nEvadeTicks > TickCount)
            {
                nEvadeTicks = 0;
                context.SetVariable("nEvadeTicks", nEvadeTicks);
                context.QRotation = Quaternion.Identity;
                context.Rotation = new Vector3(context.Rotation.X, 0.0f, context.Rotation.Z);
                context.Scale = Vector3.One;

                return BehaviorStatuses.Failure;
            }

            return BehaviorStatuses.Failure;
        }
    }
}
