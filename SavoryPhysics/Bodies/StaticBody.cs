using SpiceEngineCore.Entities;
using SavoryPhysicsCore.Shapes;

namespace SavoryPhysicsCore.Bodies
{
    public class StaticBody : Body
    {
        public StaticBody(int entityID, IShape shape) : base(entityID, shape) { }
    }
}
