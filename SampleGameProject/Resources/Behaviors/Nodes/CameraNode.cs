using OpenTK;
using OpenTK.Input;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Nodes;
using SpiceEngineCore.Utilities;

namespace SampleGameProject.Resources.Behaviors.Nodes
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
            if (context.Entity is PerspectiveCamera perspectiveCamera)
            {
                var amount = context.InputProvider.MouseWheelDelta * ZoomSpeed;

                if (amount > 0.0f || amount < 0.0f)
                {
                    perspectiveCamera.Distance += amount;

                    perspectiveCamera.CalculateTranslation();
                    perspectiveCamera.CalculateUp();
                }

                var currentAngles = perspectiveCamera.CurrentAngles;

                if (context.InputProvider.IsDown(new Input(MouseButton.Right)))
                {
                    var mouseDelta = context.InputProvider.MouseDelta * TurnSpeed;

                    if (mouseDelta != Vector2.Zero)
                    {
                        currentAngles.X += mouseDelta.X;
                        currentAngles.Y += mouseDelta.Y;
                        perspectiveCamera.CurrentAngles = currentAngles;
                        //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                        perspectiveCamera.CalculateTranslation();
                        perspectiveCamera.CalculateUp();
                    }
                }
                else
                {
                    if (context.InputProvider.IsDown(new Input(Key.Up)))
                    {
                        currentAngles.Y += MoveSpeed;
                    }

                    if (context.InputProvider.IsDown(new Input(Key.Down)))
                    {
                        currentAngles.Y -= MoveSpeed;
                    }

                    if (context.InputProvider.IsDown(new Input(Key.Right)))
                    {
                        currentAngles.X -= MoveSpeed;
                    }

                    if (context.InputProvider.IsDown(new Input(Key.Left)))
                    {
                        currentAngles.X += MoveSpeed;
                    }

                    perspectiveCamera.CurrentAngles = currentAngles;
                    //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                    perspectiveCamera.CalculateTranslation();
                    perspectiveCamera.CalculateUp();
                }
            }
            else if (context.Entity is OrthographicCamera orthographicCamera)
            {
                var amount = context.InputProvider.MouseWheelDelta * ZoomSpeed;

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
