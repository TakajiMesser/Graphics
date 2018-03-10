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
using System.Runtime.Serialization;

namespace GraphicsTest.Behaviors.Player
{
    [DataContract]
    public class MoveNode : LeafNode
    {
        [DataMember]
        public float RunSpeed { get; set; }

        [DataMember]
        public float CreepSpeed { get; set; }

        [DataMember]
        public float WalkSpeed { get; set; }

        public MoveNode(float runSpeed, float creepSpeed, float walkSpeed)
        {
            RunSpeed = runSpeed;
            CreepSpeed = creepSpeed;
            WalkSpeed = walkSpeed;
        }

        public override BehaviorStatuses Behavior(BehaviorContext context)
        {
            var speed = context.InputState.IsHeld(context.InputMapping.Run)
                ? RunSpeed
                : context.InputState.IsHeld(context.InputMapping.Crawl)
                    ? CreepSpeed
                    : WalkSpeed;

            var translation = context.GetTranslation(speed);

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
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y + 0.1f, context.Rotation.Z);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot2))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y - 0.1f, context.Rotation.Z);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot3))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y, context.Rotation.Z + 0.1f);
            }

            if (context.InputState.IsHeld(context.InputMapping.ItemSlot4))
            {
                context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y, context.Rotation.Z - 0.1f);
            }

            context.Translation = translation;
            return BehaviorStatuses.Success;
        }
    }
}
