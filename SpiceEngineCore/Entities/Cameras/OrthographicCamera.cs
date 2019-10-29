using OpenTK;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Outputs;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngineCore.Entities.Cameras
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

        public Matrix4 CalculateProjection()
        {
            var width = 0.8f;
            var height = width / _projectionMatrix.Resolution.AspectRatio;
            return Matrix4.CreateOrthographic(width, height, _projectionMatrix.ZNear, _projectionMatrix.ZFar);
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
