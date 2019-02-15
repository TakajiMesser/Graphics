using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics
{
    public abstract class Body
    {
        public int EntityID { get; }
        public IShape Shape { get; }

        public Body(int entityID, IShape shape)
        {
            EntityID = entityID;
            Shape = shape;
        }
    }
}
