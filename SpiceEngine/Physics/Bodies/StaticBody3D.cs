using SpiceEngine.Entities;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Physics.Bodies
{
    public class StaticBody3D : Body3D
    {
        public StaticBody3D(IEntity entity, IShape shape) : base(entity, shape)
        {
            
        }
    }
}
