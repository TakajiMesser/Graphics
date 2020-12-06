using OpenTK.Input;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Utilities;
using System;
using TangyHIDCore;
using TangyHIDCore.Inputs;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

namespace TowerWarfare.Resources.Behaviors.Nodes
{
    public class CameraNode : Node
    {
        public const float STRAFE_SCALE = 0.002f;
        public const float TRAVEL_SCALE = 0.02f;
        public const float TURN_SCALE = 0.001f;
        public const float ZOOM_SCALE = 0.1f;

        public const float MIN_ZOOM_POSITION = 10.0f;
        public const float MAX_ZOOM_POSITION = 100.0f;

        private float _yaw; // Yaw of zero should point in the direction of the X-Axis
        private float _pitch; // Pitch of zero should point in the direction of the X-Axis

        public CameraNode(float moveSpeed, float turnSpeed, float zoomSpeed)
        {
            MoveSpeed = moveSpeed;
            TurnSpeed = turnSpeed;
            ZoomSpeed = zoomSpeed;

            _yaw = MathExtensions.HALF_PI;
            _pitch = -MathExtensions.HALF_PI + 0.01f;
        }

        public float MoveSpeed { get; set; } //= 0.02f;
        public float TurnSpeed { get; set; } //= 0.001f;
        public float ZoomSpeed { get; set; } //= 1.0f;

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.GetEntity() is PerspectiveCamera camera)
            {
                var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();

                if (inputProvider.IsDown(new Input(MouseButton.Right)))
                {
                    Strafe(camera, inputProvider.MouseDelta);
                }
                
                if (inputProvider.IsMouseInWindow && inputProvider.MouseWheelDelta != 0)
                {
                    Zoom(camera, inputProvider.MouseWheelDelta);
                }

                if (inputProvider.IsDown(new Input(MouseButton.Button1)))
                {
                    Travel(camera, inputProvider.MouseDelta);
                }

                if (inputProvider.IsDown(new Input(MouseButton.Button2)))
                {
                    Turn(camera, inputProvider.MouseDelta);
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
                var verticalTranslation = upDirection * mouseDelta.Y * TRAVEL_SCALE;//STRAFE_SCALE * camera.Position.Z;
                var horizontalTranslation = rightDirection * mouseDelta.X * TRAVEL_SCALE;//STRAFE_SCALE * camera.Position.Z;

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

        public void Travel(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                var translation = (camera._viewMatrix.LookAt - camera.Position) * mouseDelta.Y * TRAVEL_SCALE;
                camera.Position -= translation;

                _yaw = (_yaw - mouseDelta.X * TURN_SCALE) % MathExtensions.TWO_PI;
                CalculateLookAt(camera);
                CalculateUp(camera);
            }
        }

        public void Turn(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                _yaw = (_yaw - mouseDelta.X * 0.001f) % MathExtensions.TWO_PI;
                _pitch -= mouseDelta.Y * TURN_SCALE;
                _pitch = _pitch.Clamp(-MathExtensions.HALF_PI, MathExtensions.HALF_PI);

                CalculateLookAt(camera);
                CalculateUp(camera);
            }
        }

        private void CalculateLookAt(PerspectiveCamera camera)
        {
            var lookDirection = new Vector3()
            {
                X = (float)(Math.Cos(_yaw) * Math.Cos(_pitch)),
                Y = (float)(Math.Sin(_yaw) * Math.Cos(_pitch)),
                Z = (float)Math.Sin(_pitch)
            };

            camera._viewMatrix.LookAt = camera.Position + lookDirection.Normalized();
        }

        private void CalculateUp(PerspectiveCamera camera)
        {
            var yAngle = _pitch + MathExtensions.HALF_PI;

            var upDirection = new Vector3()
            {
                X = (float)(Math.Cos(_yaw) * Math.Cos(yAngle)),
                Y = (float)(Math.Sin(_yaw) * Math.Cos(yAngle)),
                Z = (float)Math.Sin(yAngle)
            };

            camera._viewMatrix.Up = upDirection.Normalized();
        }
    }
}
