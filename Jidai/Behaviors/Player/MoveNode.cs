using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Physics.Raycasting;
using OpenTK;
using SpiceEngine.Entities;
using System.Runtime.Serialization;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Bodies;

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
            var speed = context.InputManager.IsDown(context.InputMapping.Run)
                ? RunSpeed
                : context.InputManager.IsDown(context.InputMapping.Crawl)
                    ? CreepSpeed
                    : WalkSpeed;

            var translation = GeometryHelper.GetHeldTranslation(context.Camera, speed, context.InputManager, context.InputMapping);

            if (context.InputManager.IsDown(context.InputMapping.In))
            {
                translation.Z += speed;
            }

            if (context.InputManager.IsDown(context.InputMapping.Out))
            {
                translation.Z -= speed;
            }

            if (context.InputManager.IsDown(context.InputMapping.ItemSlot1))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y + 0.1f, context.EulerRotation.Z);
            }

            if (context.InputManager.IsDown(context.InputMapping.ItemSlot2))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y - 0.1f, context.EulerRotation.Z);
            }

            if (context.InputManager.IsDown(context.InputMapping.ItemSlot3))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z + 0.1f);
            }

            if (context.InputManager.IsDown(context.InputMapping.ItemSlot4))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z - 0.1f);
            }

            ((RigidBody3D)context.Body).ApplyImpulse(translation);
            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
