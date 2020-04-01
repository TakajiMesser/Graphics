using OpenTK;
using OpenTK.Input;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Utilities;
using System;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

namespace TowerWarfare.Resources.Behaviors.Nodes
{
    public class CameraNode : Node
    {
        public const float TRANSLATE_SCALE = 0.002f;
        public const float ZOOM_SCALE = 0.1f;

        public const float MIN_ZOOM_POSITION = 10.0f;
        public const float MAX_ZOOM_POSITION = 100.0f;

        public CameraNode(float moveSpeed, float turnSpeed, float zoomSpeed)
        {
            MoveSpeed = moveSpeed;
            TurnSpeed = turnSpeed;
            ZoomSpeed = zoomSpeed;
        }

        public float MoveSpeed { get; set; } //= 0.02f;
        public float TurnSpeed { get; set; } //= 0.001f;
        public float ZoomSpeed { get; set; } //= 1.0f;

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.Entity is PerspectiveCamera camera)
            {
                if (context.InputProvider.IsDown(new Input(MouseButton.Right)))
                {
                    Strafe(camera, context.InputProvider.MouseDelta);
                }
                
                if (context.InputProvider.MouseWheelDelta != 0)
                {
                    Zoom(camera, context.InputProvider.MouseWheelDelta);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }

        public void Strafe(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                var upDirection = camera._viewMatrix.Up;
                var lookDirection = camera._viewMatrix.LookAt - camera.Position;

                var rightDirection = Vector3.Cross(upDirection, lookDirection).Normalized();

                // TODO - Improve this calculation, it should not just be using the raw Z position as a scalar
                var verticalTranslation = upDirection * mouseDelta.Y * TRANSLATE_SCALE * camera.Position.Z;
                var horizontalTranslation = rightDirection * mouseDelta.X * TRANSLATE_SCALE * camera.Position.Z;

                camera.Position += verticalTranslation + horizontalTranslation;
                camera._viewMatrix.LookAt += verticalTranslation + horizontalTranslation;
            }
        }

        public void Zoom(PerspectiveCamera camera, int mouseWheelDelta)
        {
            var translation = (camera._viewMatrix.LookAt - camera.Position) * mouseWheelDelta * ZOOM_SCALE;

            camera.Position = new Vector3()
            {
                X = camera.Position.X - translation.X,
                Y = camera.Position.Y - translation.Y,
                Z = (camera.Position.Z - translation.Z).Clamp(MIN_ZOOM_POSITION, MAX_ZOOM_POSITION)
            };
        }
    }
}
