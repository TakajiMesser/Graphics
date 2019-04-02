using OpenTK;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Scripting;
using SpiceEngine.Scripting.Nodes;
using SpiceEngine.Utilities;
using System;

namespace SampleGameProject.Resources.Behaviors.Nodes
{
    public class BlockNode : Node
    {
        public float EvadeSpeed { get; set; }
        public int TickCount { get; set; }

        public BlockNode(float evadeSpeed, int tickCount)
        {
            EvadeSpeed = evadeSpeed;
            TickCount = tickCount;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var nEvadeTicks = context.GetVariableOrDefault<int>("nEvadeTicks");

            if (nEvadeTicks > 0 && nEvadeTicks <= TickCount)
            {
                var evadeTranslation = context.GetVariable<Vector3>("evadeTranslation");

                nEvadeTicks++;
                context.SetVariable("nEvadeTicks", nEvadeTicks);

                context.Actor.Rotation = Quaternion.FromAxisAngle(Vector3.Cross(evadeTranslation.Normalized(), -Vector3.UnitZ), 2.0f * (float)Math.PI / TickCount * nEvadeTicks);
                context.Actor.Rotation *= new Quaternion((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), 0.0f, 0.0f);
                context.EulerRotation = new Vector3(context.EulerRotation.X, 2.0f * (float)Math.PI / TickCount * nEvadeTicks, context.EulerRotation.Z);

                ((RigidBody3D)context.Body).ApplyVelocity(evadeTranslation);

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
            else if (context.InputProvider.IsPressed(context.InputProvider.InputMapping.Block))
            {
                context.SetVariable("nBlockTicks", 1);

                // Swallow any directional movement inputs
                //context.InputState.SwallowInputs(context.InputProvider.InputMapping.Forward, context.InputProvider.InputMapping.Backward, context.InputProvider.InputMapping.Left, context.InputProvider.InputMapping.Right);
            }
            else if (context.InputProvider.IsDown(context.InputProvider.InputMapping.Block))
            {
                var nBlockTicks = context.GetVariableOrDefault<int>("nBlockTicks");
                nBlockTicks++;

                context.SetVariable("nBlockTicks", nBlockTicks);

                var evadeTranslation = GeometryHelper.GetPressedTranslation(context.Camera, EvadeSpeed, context.InputProvider, context.InputProvider.InputMapping);

                if (evadeTranslation.IsSignificant())
                {
                    nEvadeTicks++;

                    var angle = (float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X);

                    context.Actor.Rotation = new Quaternion(0.0f, 0.0f, (float)Math.Sin(angle / 2), (float)Math.Cos(angle / 2));
                    context.EulerRotation = new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), context.EulerRotation.Y, context.EulerRotation.Z);
                    ((RigidBody3D)context.Body).ApplyVelocity(evadeTranslation);

                    context.SetVariable("evadeTranslation", evadeTranslation);
                    context.SetVariable("nEvadeTicks", nEvadeTicks);

                    return BehaviorStatus.Success;
                }
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
