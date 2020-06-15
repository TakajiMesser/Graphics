using SavoryPhysicsCore.Shapes;
using SpiceEngineCore.Entities;

namespace SavoryPhysicsCore.Collisions
{
    public class CollisionInfo
    {
        public CollisionInfo(IEntity entityA, IEntity entityB, IBody bodyA, IBody bodyB)
        {
            EntityA = entityA;
            EntityB = entityB;
            BodyA = bodyA;
            BodyB = bodyB;
        }

        public IEntity EntityA { get; }
        public IEntity EntityB { get; }

        public IBody BodyA { get; }
        public IBody BodyB { get; }

        public bool IsPhysical => BodyA.IsPhysical && BodyB.IsPhysical;

        public bool AreShape<T>() where T : IShape => BodyA.Shape is T && BodyB.Shape is T;
        public bool AreShapes<T, U>() where T : IShape where U : IShape => BodyA.Shape is T && BodyB.Shape is U;

        public CollisionInfo Flipped() => new CollisionInfo(EntityB, EntityA, BodyB, BodyA);
    }
}
