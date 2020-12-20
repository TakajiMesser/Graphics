using OpenTK;
using SpiceEngineCore.Rendering.Matrices;

namespace SpiceEngineCore.Entities.Cameras
{
    public class OrthographicCamera : Camera
    {
        public const float ZNEAR = -100.0f;
        public const float ZFAR = 100.0f;

        public OrthographicCamera(string name, float zNear, float zFar, float startingWidth) : base(name, ProjectionTypes.Orthographic) => _projectionMatrix.InitializeOrthographic(startingWidth, zNear, zFar);

        public float Width
        {
            get => _projectionMatrix.Width;
            set => _projectionMatrix.Width = value;
        }

        public Matrix4 CalculateProjection()
        {
            // TODO - What is this magic number?
            var width = 0.8f;
            var height = width / _projectionMatrix.AspectRatio;
            return Matrix4.CreateOrthographic(width, height, _projectionMatrix.ZNear, _projectionMatrix.ZFar);
        }
    }
}
