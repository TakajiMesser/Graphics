using SpiceEngineCore.Geometry.Vectors;

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
