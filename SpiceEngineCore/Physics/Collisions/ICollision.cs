namespace SpiceEngineCore.Physics.Collisions
{
    public interface ICollision
    {
        float PenetrationDepth { get; set; }
        bool HasCollision { get; }
    }
}
