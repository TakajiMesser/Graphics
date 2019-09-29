namespace SpiceEngineCore.Physics.Collisions
{
    public class Bounds
    {
        public int EntityID { get; }
        public IPartition Partition { get; }

        public Bounds(int entityID, IPartition partition)
        {
            EntityID = entityID;
            Partition = partition;
        }
    }
}
