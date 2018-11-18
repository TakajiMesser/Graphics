using SpiceEngine.Inputs;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Matrices;

namespace SpiceEngine.Entities.Cameras
{
    public class OrthographicCamera : Camera
    {
        public const float ZNEAR = -100.0f;
        public const float ZFAR = 100.0f;

        public float Width
        {
            get => _projectionMatrix.Width;
            set => _projectionMatrix.Width = value;
        }

        public OrthographicCamera(string name, Resolution resolution, float zNear, float zFar, float startingWidth) : base(name)
        {
            _projectionMatrix = new ProjectionMatrix(ProjectionTypes.Orthographic, resolution);
            _projectionMatrix.UpdateOrthographic(startingWidth, zNear, zFar);
        }

        public override void OnHandleInput(InputManager inputManager)
        {
            float amount = inputManager.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _projectionMatrix.Width += amount;
            }
        }
    }
}
