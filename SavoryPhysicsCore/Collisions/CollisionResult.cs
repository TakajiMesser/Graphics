namespace SavoryPhysicsCore.Collisions
{
    public class CollisionResult
    {
        private CollisionResult(CollisionInfo collisionInfo, Collision collision, bool hasCollision)
        {
            CollisionInfo = collisionInfo;
            Collision = collision;
            HasCollision = hasCollision;
        }

        public CollisionInfo CollisionInfo { get; }
        public Collision Collision { get; }
        public bool HasCollision { get; }

        public CollisionPair ConstructCollisionPair() => new CollisionPair(CollisionInfo.BodyA.EntityID, CollisionInfo.BodyB.EntityID);

        public static CollisionResult NoCollision(CollisionInfo collisionInfo) => new CollisionResult(collisionInfo, null, false);
        public static CollisionResult FullCollision(CollisionInfo collisionInfo, Collision collision) => new CollisionResult(collisionInfo, collision, true);
        public static CollisionResult LimitedCollision(CollisionInfo collisionInfo) => new CollisionResult(collisionInfo, null, true);
    }
}
