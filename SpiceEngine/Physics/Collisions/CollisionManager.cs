using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public class CollisionManager
    {
        private HashSet<CollisionPair> _broadCollisions = new HashSet<CollisionPair>();

        private Dictionary<int, HashSet<CollisionPair>> _narrowCollisionPairsByEntityID = new Dictionary<int, HashSet<CollisionPair>>();
        private Dictionary<CollisionPair, Collision3D> _narrowCollisionByCollisionPair = new Dictionary<CollisionPair, Collision3D>();

        public IEnumerable<CollisionPair> BroadCollisionPairs => _broadCollisions;
        public IEnumerable<int> NarrowCollisionIDs => _narrowCollisionPairsByEntityID.Keys;
        public IEnumerable<Collision3D> NarrowCollisions => _narrowCollisionByCollisionPair.Values;

        public IEnumerable<int> GetNarrowCollisionIDs(int entityID)
        {
            var entityIDs = new HashSet<int>();

            foreach (var collisionPair in _narrowCollisionPairsByEntityID[entityID])
            {
                entityIDs.Add(collisionPair.FirstEntityID);
                entityIDs.Add(collisionPair.SecondEntityID);
            }

            return entityIDs;
        }

        public IEnumerable<Collision3D> GetNarrowCollisions(int entityID)
        {
            if (_narrowCollisionPairsByEntityID.ContainsKey(entityID))
            {
                foreach (var collisionPair in _narrowCollisionPairsByEntityID[entityID])
                {
                    yield return _narrowCollisionByCollisionPair[collisionPair];
                }
            }
        }

        public void AddBroadCollision(int entityID, IEnumerable<int> colliderIDs)
        {
            foreach (var colliderID in colliderIDs)
            {
                _broadCollisions.Add(new CollisionPair(entityID, colliderID));
            }
        }

        public void AddNarrowCollision(Collision3D collision)
        {
            var collisionPair = new CollisionPair(collision.FirstBody.EntityID, collision.SecondBody.EntityID);

            _narrowCollisionByCollisionPair.Add(collisionPair, collision);

            if (!_narrowCollisionPairsByEntityID.ContainsKey(collisionPair.FirstEntityID))
            {
                _narrowCollisionPairsByEntityID.Add(collisionPair.FirstEntityID, new HashSet<CollisionPair>());
            }

            if (!_narrowCollisionPairsByEntityID.ContainsKey(collisionPair.SecondEntityID))
            {
                _narrowCollisionPairsByEntityID.Add(collisionPair.SecondEntityID, new HashSet<CollisionPair>());
            }

            _narrowCollisionPairsByEntityID[collisionPair.FirstEntityID].Add(collisionPair);
            _narrowCollisionPairsByEntityID[collisionPair.SecondEntityID].Add(collisionPair);
        }

        public void Clear()
        {
            _broadCollisions.Clear();
            _narrowCollisionPairsByEntityID.Clear();
            _narrowCollisionByCollisionPair.Clear();
        }
    }
}
