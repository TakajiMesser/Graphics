namespace SpiceEngine.Physics.Collision
{
    public class CollisionManager
    {
        private HashSet<CollisionPair> _broadCollisions = new HashSet<CollisionPair>();
        private HashSet<CollisionPair> _narrowCollisions = new HashSet<CollisionPair>();
        private Dictionary<int, HashSet<int>> _narrowCollisionIDsByEntityID = new Dictionary<int, HashSet<int>>();

        public IEnumerable<CollisionPair> BroadCollisions => _broadCollisions;
        public IEnumerable<CollisionPair> NarrowCollisions => _narrowCollisions;

        public IEnumerable<int> GetNarrowCollisions(int entityID)
        {
            if (_narrowCollisionIDsByEntityID.ContainsKey(entityID))
            {
                return _narrowCollisionIDsByEntityID[entityID];
            }
            else
            {
                return Enumerable.Empty<int>();
            }
        }

        public void AddBroadCollision(int entityID, IEnumerable<Bounds> bounds)
        {
            foreach (var bound in bounds)
            {
                _broadCollisions.Add(entityID, bound.EntityID);
            }
        }

        public void AddNarrowCollision(CollisionPair collisionPair)
        {
            _narrowCollisions.Add(collisionPair);

            if (!_narrowCollisionIDsByEntityID.ContainsKey(collisionPair.FirstEntityID))
            {
                _narrowCollisionIDsByEntityID.Add(collisionPair.FirstEntityID, new HashSet<int>());
            }

            _narrowCollisionIDsByEntityID[collisionPair.FirstEntityID].Add(collisionPair.SecondEntityID);

            if (!_narrowCollisionIDsByEntityID.ContainsKey(collisionPair.SecondEntityID))
            {
                _narrowCollisionIDsByEntityID.Add(collisionPair.SecondEntityID, new HashSet<int>());
            }

            _narrowCollisionIDsByEntityID[collisionPair.SecondEntityID].Add(collisionPair.FirstEntityID);
        }

        public void Clear()
        {
            _broadCollisions.Clear();
            _narrowCollisions.Clear();
            _narrowCollisionIDsByEntityID.Clear();
        }
    }
}
