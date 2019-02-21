using OpenTK;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics.Bodies
{
    public abstract class Body3D : IBody
    {
        public int EntityID { get; }
        public IShape Shape { get; }
        public Vector3 Position { get; }

        public Body3D(IEntity entity, IShape shape)
        {
            EntityID = entity.ID;
            Shape = shape;
            Position = entity.Position;
        }

        public Collision GetCollision(Body3D body)
        {
            
        }
    }
}
