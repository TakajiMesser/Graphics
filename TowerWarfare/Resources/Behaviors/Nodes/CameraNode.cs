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
        public const float MIN_DISTANCE = 1.0f;
        public const float MIN_PITCH = -MathExtensions.HALF_PI + 0.01f;
        public const float MAX_PITCH = MathExtensions.HALF_PI + 0.01f;
        public const float MAX_YAW = MathExtensions.TWO_PI;

        private float _yaw; // Yaw of zero should point in the direction of the X-Axis
        private float _pitch; // Pitch of zero should point in the direction of the X-Axis

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
                if (context.InputProvider.IsDown(new Input(MouseButton.Button1)) && context.InputProvider.IsDown(new Input(MouseButton.Button2)))
                {
                    Strafe(camera, context.InputProvider.MouseDelta);
                }
                else if (context.InputProvider.IsDown(new Input(MouseButton.Button1)))
                {
                    Travel(camera, context.InputProvider.MouseDelta);
                }
                else if (context.InputProvider.IsDown(new Input(MouseButton.Button2)))
                {
                    Turn(camera, context.InputProvider.MouseDelta);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }

        public void Travel(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                var translation = (camera._viewMatrix.LookAt - camera.Position) * mouseDelta.Y * 0.02f;
                camera.Position -= translation;

                _yaw = (_yaw - mouseDelta.X * 0.001f) % MAX_YAW;
                CalculateLookAt(camera);
                CalculateUp(camera);
            }
        }

        public void Turn(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                _yaw = (_yaw - mouseDelta.X * 0.001f) % MAX_YAW;
                _pitch -= mouseDelta.Y * 0.001f;
                _pitch = _pitch.Clamp(MIN_PITCH, MAX_PITCH);

                CalculateLookAt(camera);
                CalculateUp(camera);
            }
        }

        public void Strafe(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                var upDirection = camera._viewMatrix.Up;
                var lookDirection = camera._viewMatrix.LookAt - camera.Position;

                var rightDirection = Vector3.Cross(upDirection, lookDirection).Normalized();

                var verticalTranslation = upDirection * mouseDelta.Y * 0.02f;
                var horizontalTranslation = rightDirection * mouseDelta.X * 0.02f;

                camera.Position -= verticalTranslation + horizontalTranslation;
                camera._viewMatrix.LookAt -= verticalTranslation + horizontalTranslation;
            }
        }

        public void Pivot(PerspectiveCamera camera, Vector2 mouseDelta, int mouseWheelDelta, Vector3 position)
        {
            if (mouseDelta != Vector2.Zero || mouseWheelDelta != 0)
            {
                // Determine new yaw and pitch
                var lookDirection = (position - camera.Position).Normalized();
                _pitch = (float)Math.Asin(lookDirection.Z);
                _yaw = ((float)Math.Atan2(lookDirection.Y, lookDirection.X) + MAX_YAW) % MAX_YAW;


                if (mouseWheelDelta != 0.0f)
                {
                    var translation = lookDirection * mouseWheelDelta * 1.0f;
                    camera.Position -= translation;
                }

                if (mouseDelta != Vector2.Zero)
                {
                    // Now, we can adjust our position accordingly
                    _yaw = (_yaw + mouseDelta.X * 0.001f) % MAX_YAW;//_yaw += mouseDelta.X * 0.001f;
                    _pitch += mouseDelta.Y * 0.001f;
                    _pitch = _pitch.Clamp(MIN_PITCH, MAX_PITCH);
                }

                CalculateTranslation(camera, position);
                CalculateUp(camera);
            }
        }

        private void CalculateTranslation(PerspectiveCamera camera, Vector3 position)
        {
            var distance = (camera.Position - position).Length;

            var translation = new Vector3()
            {
                X = distance * (float)(Math.Cos(_pitch) * Math.Cos(_yaw)),
                Y = distance * (float)(Math.Cos(_pitch) * Math.Sin(_yaw)),
                Z = distance * (float)Math.Sin(_pitch)
            };

            camera.Position = position - translation;
            camera._viewMatrix.LookAt = camera.Position + translation / distance;
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
