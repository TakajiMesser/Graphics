using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics
{
    public abstract class PhysicsBody
    {
        public int EntityID { get; }
        public IShape Shape { get; }

        public PhysicsBody(int entityID, IShape shape)
        {
            EntityID = entityID;
            Shape = shape;
        }
    }
}
