namespace SavoryPhysicsCore.Partitioning
{
    public interface IPartition
    {
        float Length { get; }
        bool CanContain(Bounds bounds);
    }
}
