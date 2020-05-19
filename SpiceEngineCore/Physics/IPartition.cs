namespace SpiceEngineCore.Physics
{
    public interface IPartition
    {
        float Length { get; }
        bool CanContain(Bounds bounds);
    }
}
