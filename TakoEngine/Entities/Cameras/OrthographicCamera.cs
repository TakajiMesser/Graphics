using TakoEngine.Inputs;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Matrices;

namespace TakoEngine.Entities.Cameras
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

        public override void OnHandleInput(InputState inputState)
        {
            float amount = inputState.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _projectionMatrix.Width += amount;
            }
        }
    }
}
