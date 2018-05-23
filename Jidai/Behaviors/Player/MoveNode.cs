using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;
using TakoEngine.Physics.Raycasting;
using OpenTK;
using TakoEngine.Entities;
using System.Runtime.Serialization;
using TakoEngine.Helpers;

namespace Jidai.Behaviors.Player
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
            var speed = context.InputState.IsHeld(context.InputMapping.Run)
                ? RunSpeed
                : context.InputState.IsHeld(context.InputMapping.Crawl)
                    ? CreepSpeed
                    : WalkSpeed;

            var translation = GeometryHelper.GetHeldTranslation(context.Camera, speed, context.InputState, context.InputMapping);

            if (context.InputState.IsHeld(context.InputMapping.In))
            {
                translation.Z += speed;
            }

            if (context.InputState.IsHeld(context.InputMapping.Out))
            {
                translation.Z -= speed;
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot1))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y + 0.1f, context.EulerRotation.Z);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot2))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y - 0.1f, context.EulerRotation.Z);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot3))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z + 0.1f);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot4))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z - 0.1f);
            }

            context.Translation = translation;
            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
