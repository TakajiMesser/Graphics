namespace SpiceEngine.Physics.Collision
{
    public class Bounds
    {
        public int EntityID { get; }
        public IPartition Collider { get; }

        public Bounds(int entityID, IPartition collider)
        {
            EntityID = entityID;
            Collider = collider;
        }
    }
}
