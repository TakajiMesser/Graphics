using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities.Volumes
{
    /// <summary>
    /// Use PhysicsVolumes to give all entities within the Volume unifying forces
    /// </summary>
    public class PhysicsVolume : Volume
    {
        public Vector3 Gravity { get; set; }

        public PhysicsVolume() { }
        /*public PhysicsVolume(List<Vector3> vertices, List<int> triangleIndices, Color4 color) : base(vertices, triangleIndices, color)
        {

        }*/
    }
}
