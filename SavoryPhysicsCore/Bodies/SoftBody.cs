using OpenTK;
using SpiceEngineCore.Entities;
using SavoryPhysicsCore.Shapes;

namespace SavoryPhysicsCore.Bodies
{
    public class SoftBody : Body
    {
        public SoftBody(int entityID, IShape shape) : base(entityID, shape) { }
    }
}
