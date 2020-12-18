using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Utilities;

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

        public Matrix4 Projection => Matrix4.Perspective(UnitConversions.ToRadians(90.0f), 1.0f, 0.1f, Radius);

        public override PLight ToStruct() => new PLight(Position, Radius, Color.Xyz, Intensity);
    }
}
