using OpenTK;

namespace SpiceEngine.Entities.Volumes
{
    /// <summary>
    /// Use PhysicsVolumes to give all entities within the Volume unifying forces
    /// </summary>
    public class PhysicsVolume : Volume
    {
        public Vector3 Gravity { get; set; }
    }
}
