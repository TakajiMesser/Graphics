//using OpenTK;
using OpenTK.Input;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Utilities;
using System;
using TangyHIDCore;
using TangyHIDCore.Inputs;
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

namespace TowerWarfare.Resources.Behaviors.Nodes
{
    public class PerspectiveCameraNode : Node
    {
        public const float MIN_DISTANCE = 1.0f;
        public const float MIN_PITCH = -MathExtensions.HALF_PI + 0.01f;
        public const float MAX_PITCH = MathExtensions.HALF_PI + 0.01f;
        public const float MAX_YAW = MathExtensions.TWO_PI;

        private float _yaw; // Yaw of zero should point in the direction of the X-Axis
        private float _pitch; // Pitch of zero should point in the direction of the X-Axis

        public PerspectiveCameraNode(float moveSpeed, float turnSpeed, float zoomSpeed)
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

                if (inputProvider.IsDown(new Input(MouseButton.Button1)) && inputProvider.IsDown(new Input(MouseButton.Button2)))
                {
                    Strafe(camera, inputProvider.MouseDelta);
                }
                else if (inputProvider.IsDown(new Input(MouseButton.Button1)))
                {
                    Travel(camera, inputProvider.MouseDelta);
                }
                else if (inputProvider.IsDown(new Input(MouseButton.Button2)))
                {
                    Turn(camera, inputProvider.MouseDelta);
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }

        public void Travel(PerspectiveCamera camera, Vector2 mouseDelta)
        {
            if (mouseDelta != Vector2.Zero)
            {
                var translation = (camera.LookAt - camera.Position) * mouseDelta.Y * 0.02f;
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
                var upDirection = camera.Up;
                var lookDirection = camera.LookAt - camera.Position;

                var rightDirection = Vector3.Cross(upDirection, lookDirection).Normalized();

                var verticalTranslation = upDirection * mouseDelta.Y * 0.02f;
                var horizontalTranslation = rightDirection * mouseDelta.X * 0.02f;

                camera.Position -= verticalTranslation + horizontalTranslation;
                camera.LookAt -= verticalTranslation + horizontalTranslation;
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

            var translation = new Vector3(
                distance * (float)(Math.Cos(_pitch) * Math.Cos(_yaw)),
                distance * (float)(Math.Cos(_pitch) * Math.Sin(_yaw)),
                distance * (float)Math.Sin(_pitch)
            );

            camera.Position = position - translation;
            camera.LookAt = camera.Position + translation / distance;
        }

        private void CalculateLookAt(PerspectiveCamera camera)
        {
            var lookDirection = new Vector3(
                (float)(Math.Cos(_yaw) * Math.Cos(_pitch)),
                (float)(Math.Sin(_yaw) * Math.Cos(_pitch)),
                (float)Math.Sin(_pitch)
            );

            camera.LookAt = camera.Position + lookDirection.Normalized();
        }

        private void CalculateUp(PerspectiveCamera camera)
        {
            var yAngle = _pitch + MathExtensions.HALF_PI;

            var upDirection = new Vector3(
                (float)(Math.Cos(_yaw) * Math.Cos(yAngle)),
                (float)(Math.Sin(_yaw) * Math.Cos(yAngle)),
                (float)Math.Sin(yAngle)
            );

            camera.Up = upDirection.Normalized();
        }
    }
}
