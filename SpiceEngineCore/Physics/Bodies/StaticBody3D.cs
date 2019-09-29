using SpiceEngineCore.Entities;
using SpiceEngineCore.Physics.Shapes;

namespace SpiceEngineCore.Physics.Bodies
{
    public class StaticBody3D : Body3D
    {
        public StaticBody3D(IEntity entity, Shape3D shape) : base(entity, shape)
        {
            
        }
    }
}
