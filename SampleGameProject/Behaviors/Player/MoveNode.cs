using OpenTK;
using SpiceEngine.Helpers;
using SpiceEngine.Scripting;
using SpiceEngine.Scripting.Nodes;
using SpiceEngineCore.Physics.Bodies;
using SpiceEngineCore.Utilities;

namespace SampleGameProject.Behaviors.Player
{
    public class MoveNode : Node
    {
        public float RunSpeed { get; set; }
        public float CreepSpeed { get; set; }
        public float WalkSpeed { get; set; }

        public MoveNode(float runSpeed, float creepSpeed, float walkSpeed)
        {
            RunSpeed = runSpeed;
            CreepSpeed = creepSpeed;
            WalkSpeed = walkSpeed;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var speed = context.InputProvider.IsDown(context.InputProvider.InputMapping.Run)
                ? RunSpeed
                : context.InputProvider.IsDown(context.InputProvider.InputMapping.Crawl)
                    ? CreepSpeed
                    : WalkSpeed;

            var translation = GeometryHelper.GetHeldTranslation(context.Camera, speed, context.InputProvider, context.InputProvider.InputMapping);

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.In))
            {
                translation.Z += speed;
            }

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.Out))
            {
                translation.Z -= speed;
            }

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.Evade))
            {
                translation.Z += 0.6f;
            }

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.ItemSlot1))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y + 0.1f, context.EulerRotation.Z);
            }

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.ItemSlot2))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y - 0.1f, context.EulerRotation.Z);
            }

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.ItemSlot3))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z + 0.1f);
            }

            if (context.InputProvider.IsDown(context.InputProvider.InputMapping.ItemSlot4))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z - 0.1f);
            }

            //if (translation.IsSignificant())
            //{
                ((RigidBody3D)context.Body).ApplyVelocity(translation);
            //}
            
            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
