namespace SpiceEngineCore.Physics
{
    public interface ICollision
    {
        float PenetrationDepth { get; set; }
        bool HasCollision { get; }
    }
}
