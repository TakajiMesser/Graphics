namespace SpiceEngine.Physics.Collision
{
    public interface IPartition
    {
        float Length { get; }
        bool CanContain(Bounds bounds);
    }
}
