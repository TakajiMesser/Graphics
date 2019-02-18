using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public class CollisionManager
    {
        private HashSet<CollisionPair> _broadCollisions = new HashSet<CollisionPair>();

        private Dictionary<int, HashSet<CollisionPair>> _narrowCollisionPairsByEntityID = new Dictionary<int, HashSet<CollisionPair>>();
        private Dictionary<CollisionPair, Collision> _narrowCollisionByCollisionPair = new Dictionary<CollisionPair, Collision>();

        public IEnumerable<CollisionPair> BroadCollisionPairs => _broadCollisions;
        public IEnumerable<int> NarrowCollisionIDs => _narrowCollisionPairsByEntityID.Keys;
        public IEnumerable<Collision> NarrowCollisions => _narrowCollisionByCollisionPair.Values;

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

        public IEnumerable<Collision> GetNarrowCollisions(int entityID)
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

        public void AddNarrowCollision(Collision collision)
        {
            _narrowCollisionByCollisionPair.Add(collision.CollisionPair, collision);

            if (!_narrowCollisionPairsByEntityID.ContainsKey(collision.CollisionPair.FirstEntityID))
            {
                _narrowCollisionPairsByEntityID.Add(collision.CollisionPair.FirstEntityID, new HashSet<CollisionPair>());
            }

            if (!_narrowCollisionPairsByEntityID.ContainsKey(collision.CollisionPair.SecondEntityID))
            {
                _narrowCollisionPairsByEntityID.Add(collision.CollisionPair.SecondEntityID, new HashSet<CollisionPair>());
            }

            _narrowCollisionPairsByEntityID[collision.CollisionPair.FirstEntityID].Add(collision.CollisionPair);
            _narrowCollisionPairsByEntityID[collision.CollisionPair.SecondEntityID].Add(collision.CollisionPair);
        }

        public void Clear()
        {
            _broadCollisions.Clear();
            _narrowCollisionPairsByEntityID.Clear();
            _narrowCollisionByCollisionPair.Clear();
        }
    }
}
