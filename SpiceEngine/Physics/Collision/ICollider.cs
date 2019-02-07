namespace SpiceEngine.Physics.Collision
{
    public interface ICollider
    {
        float Length { get; }
        bool CanContain(Bounds bounds);
    }
}
