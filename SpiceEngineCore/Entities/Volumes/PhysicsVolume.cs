using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
