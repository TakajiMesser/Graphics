using OpenTK.Input;
using SpiceEngineCore.Entities.Cameras;
using TangyHIDCore;
using TangyHIDCore.Inputs;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace UmamiScriptingCore.Behaviors.Nodes.Leaves
{
    public class CameraNode : Node
    {
        public float MoveSpeed { get; set; } //= 0.02f;
        public float TurnSpeed { get; set; } //= 0.001f;
        public float ZoomSpeed { get; set; } //= 1.0f;

        public CameraNode(float moveSpeed, float turnSpeed, float zoomSpeed)
        {
            MoveSpeed = moveSpeed;
            TurnSpeed = turnSpeed;
            ZoomSpeed = zoomSpeed;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();

            if (context.GetEntity() is PerspectiveCamera perspectiveCamera)
            {
                var amount = inputProvider.MouseWheelDelta * ZoomSpeed;

                if (amount > 0.0f || amount < 0.0f)
                {
                    perspectiveCamera.Distance += amount;

                    perspectiveCamera.CalculateTranslation();
                    perspectiveCamera.CalculateUp();
                }

                var currentX = perspectiveCamera.CurrentAngles.X;
                var currentY = perspectiveCamera.CurrentAngles.Y;
                var currentZ = perspectiveCamera.CurrentAngles.Z;

                if (inputProvider.IsDown(new Input(MouseButton.Right)))
                {
                    var mouseDelta = inputProvider.MouseDelta * TurnSpeed;

                    if (mouseDelta != Vector2.Zero)
                    {
                        currentX += mouseDelta.X;
                        currentY += mouseDelta.Y;
                        perspectiveCamera.CurrentAngles = new Vector3(currentX, currentY, currentZ);
                        //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                        perspectiveCamera.CalculateTranslation();
                        perspectiveCamera.CalculateUp();
                    }
                }
                else
                {
                    if (inputProvider.IsDown(new Input(Key.Up)))
                    {
                        currentY += MoveSpeed;
                    }

                    if (inputProvider.IsDown(new Input(Key.Down)))
                    {
                        currentY -= MoveSpeed;
                    }

                    if (inputProvider.IsDown(new Input(Key.Right)))
                    {
                        currentX -= MoveSpeed;
                    }

                    if (inputProvider.IsDown(new Input(Key.Left)))
                    {
                        currentX += MoveSpeed;
                    }

                    perspectiveCamera.CurrentAngles = new Vector3(currentX, currentY, currentZ);
                    //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                    perspectiveCamera.CalculateTranslation();
                    perspectiveCamera.CalculateUp();
                }
            }
            else if (context.GetEntity() is OrthographicCamera orthographicCamera)
            {
                var amount = inputProvider.MouseWheelDelta * ZoomSpeed;

                if (amount > 0.0f || amount < 0.0f)
                {
                    orthographicCamera.Width += amount;
                }
            }

            return BehaviorStatus.Success;
        }

        public override void Reset() { }
    }
}
