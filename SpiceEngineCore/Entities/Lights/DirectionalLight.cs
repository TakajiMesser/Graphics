using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Quaternions;
using SpiceEngineCore.Geometry.Vectors;

namespace SpiceEngineCore.Entities.Lights
{
    public class DirectionalLight : Light<DLight>, IRotate
    {
        public Quaternion Rotation { get; set; }

        public Vector3 Direction => (new Vector4(0.0f, 0.0f, -1.0f, 1.0f) * Matrix4.CreateFromQuaternion(Rotation)).Xyz;
        public Matrix4 View => Matrix4.LookAt(Vector3.Zero, Vector3.Zero + Direction.Normalized(), Vector3.UnitZ);

        //public Matrix4 GetProjection(Resolution resolution) => Matrix4.CreateOrthographic(resolution.Width, resolution.Height, 0.1f, 100.0f);

        public void Rotate(Quaternion rotation) => Rotation = rotation * Rotation;

        public override DLight ToStruct() => new DLight(Color.Xyz, Intensity);
    }
}
