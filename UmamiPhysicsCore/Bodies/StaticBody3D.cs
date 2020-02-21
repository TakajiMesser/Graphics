using SpiceEngineCore.Entities;
using SavoryPhysicsCore.Shapes;

namespace SavoryPhysicsCore.Bodies
{
    public class StaticBody3D : Body3D
    {
        public StaticBody3D(IEntity entity, Shape3D shape) : base(entity, shape)
        {
            
        }
    }
}
