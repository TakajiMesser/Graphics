using SpiceEngineCore.Utilities;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Entities.Lights
{
    public class PointLight : Light<PLight>
    {
        private float _radius = 1.0f;

        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                _modelMatrix.Scale = Vector3.One * _radius;
            }
        }

        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView(UnitConversions.ToRadians(90.0f), 1.0f, 0.1f, Radius);

        public override PLight ToStruct() => new PLight(Position, Radius, Color.Xyz, Intensity);
    }
}
