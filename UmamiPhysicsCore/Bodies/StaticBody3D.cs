using SpiceEngineCore.Entities;
using UmamiPhysicsCore.Shapes;

namespace UmamiPhysicsCore.Bodies
{
    public class StaticBody3D : Body3D
    {
        public StaticBody3D(IEntity entity, Shape3D shape) : base(entity, shape)
        {
            
        }
    }
}
