using SpiceEngineCore.Rendering.Matrices;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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

        public OrthographicCamera(string name, float zNear, float zFar, float startingWidth) : base(name, ProjectionTypes.Orthographic) =>
            _projectionMatrix.UpdateOrthographic(startingWidth, zNear, zFar);

        public Matrix4 CalculateProjection()
        {
            // TODO - What is this magic number?
            var width = 0.8f;
            var height = width / _projectionMatrix.AspectRatio;

            return Matrix4.CreateOrthographic(width, height, _projectionMatrix.ZNear, _projectionMatrix.ZFar);
        }
    }
}
