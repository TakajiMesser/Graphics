using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using SpiceEngine.Helpers;
using System.Linq;
using TangyHIDCore;
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

namespace SampleGameProject.Resources.Behaviors.Nodes
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
            var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();
            var speed = inputProvider.IsDown(inputProvider.InputMapping.Run)
                ? RunSpeed
                : inputProvider.IsDown(inputProvider.InputMapping.Crawl)
                    ? CreepSpeed
                    : WalkSpeed;

            var translation = GeometryHelper.GetHeldTranslation(context.SystemProvider.EntityProvider.Cameras.First(c => c.IsActive), speed, inputProvider);

            if (inputProvider.IsDown(inputProvider.InputMapping.In))
            {
                translation = new Vector3(translation.X, translation.Y, translation.Z + speed);
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Out))
            {
                translation = new Vector3(translation.X, translation.Y, translation.Z - speed);
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.Evade))
            {
                translation = new Vector3(translation.X, translation.Y, translation.Z + 0.6f);
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.ItemSlot1))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y + 0.1f, context.EulerRotation.Z);
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.ItemSlot2))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y - 0.1f, context.EulerRotation.Z);
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.ItemSlot3))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z + 0.1f);
            }

            if (inputProvider.IsDown(inputProvider.InputMapping.ItemSlot4))
            {
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, context.EulerRotation.Z - 0.1f);
            }

            //if (translation.IsSignificant())
            //{
                var body = context.GetComponent<IBody>() as RigidBody;
                body.ApplyVelocity(translation);
            //}

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
