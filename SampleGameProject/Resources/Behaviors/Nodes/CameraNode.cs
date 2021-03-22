using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngineCore.Entities.Cameras;
using TangyHIDCore;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;
using Vector2 = SpiceEngineCore.Geometry.Vector2;

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

                var currentAngles = perspectiveCamera.CurrentAngles;

                if (inputProvider.IsDown(MouseButtons.Right))
                {
                    var mouseDelta = inputProvider.MouseDelta * TurnSpeed;

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
                    if (inputProvider.IsDown(Keys.Up))
                    {
                        currentAngles.Y += MoveSpeed;
                    }

                    if (inputProvider.IsDown(Keys.Down))
                    {
                        currentAngles.Y -= MoveSpeed;
                    }

                    if (inputProvider.IsDown(Keys.Right))
                    {
                        currentAngles.X -= MoveSpeed;
                    }

                    if (inputProvider.IsDown(Keys.Left))
                    {
                        currentAngles.X += MoveSpeed;
                    }

                    perspectiveCamera.CurrentAngles = currentAngles;
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
