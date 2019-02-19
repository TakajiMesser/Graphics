using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics
{
    public class StaticBody3D : Body
    {
        public Vector3 Position { get; set; }
        
        public StaticBody3D(IEntity entity, IShape shape) : base(entity.ID, shape)
        {
            Position = entity.Position;
        }
    }
}
