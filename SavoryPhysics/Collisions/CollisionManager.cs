using SavoryPhysicsCore.Helpers;
using SavoryPhysicsCore.Shapes.ThreeDimensional;
using System.Collections.Generic;

namespace SavoryPhysicsCore.Collisions
{
    public class CollisionManager
    {
        private HashSet<CollisionPair> _broadCollisions = new HashSet<CollisionPair>();

        private Dictionary<int, HashSet<CollisionPair>> _narrowCollisionPairsByEntityID = new Dictionary<int, HashSet<CollisionPair>>();
        private Dictionary<CollisionPair, CollisionResult> _narrowCollisionByCollisionPair = new Dictionary<CollisionPair, CollisionResult>();

        public IEnumerable<CollisionPair> BroadCollisionPairs => _broadCollisions;

        public IEnumerable<CollisionResult> NarrowCollisions => _narrowCollisionByCollisionPair.Values;
        public IEnumerable<int> NarrowCollisionIDs => _narrowCollisionPairsByEntityID.Keys;

        public IEnumerable<IBody> GetNarrowCollisionBodies(int entityID)
        {
            if (_narrowCollisionPairsByEntityID.ContainsKey(entityID))
            {
                foreach (var collisionPair in _narrowCollisionPairsByEntityID[entityID])
                {
                    var collisionInfo = _narrowCollisionByCollisionPair[collisionPair].CollisionInfo;

                    if (collisionInfo.BodyA.EntityID != entityID)
                    {
                        yield return collisionInfo.BodyA;
                    }

                    if (collisionInfo.BodyB.EntityID != entityID)
                    {
                        yield return collisionInfo.BodyB;
                    }
                }
            }
        }

        public CollisionResult PerformCollisionCheck(CollisionInfo collisionInfo)
        {
            if (collisionInfo.AreShape<Sphere>())
            {
                if (collisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetSphereSphereCollision(collisionInfo);
                }
                else if (CollisionHelper.HasSphereSphereCollision(collisionInfo))
                {
                    return CollisionResult.LimitedCollision(collisionInfo);
                }
            }
            else if (collisionInfo.AreShape<Box>())
            {
                if (collisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetBoxBoxCollision(collisionInfo);
                }
                else if (CollisionHelper.HasBoxBoxCollision(collisionInfo))
                {
                    return CollisionResult.LimitedCollision(collisionInfo);
                }
            }
            else if (collisionInfo.AreShape<Polyhedron>())
            {
                if (collisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetPolyhedronPolyhedronCollision(collisionInfo);
                }
                else if (CollisionHelper.HasPolyhedronPolyhedronCollision(collisionInfo))
                {
                    return CollisionResult.LimitedCollision(collisionInfo);
                }
            }

            if (collisionInfo.AreShapes<Sphere, Box>())
            {
                if (collisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetSphereBoxCollision(collisionInfo);
                }
                else if (CollisionHelper.HasSphereBoxCollision(collisionInfo))
                {
                    return CollisionResult.LimitedCollision(collisionInfo);
                }
            }
            else if (collisionInfo.AreShapes<Box, Sphere>())
            {
                var flippedCollisionInfo = collisionInfo.Flipped();

                if (flippedCollisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetSphereBoxCollision(flippedCollisionInfo);
                }
                else if (CollisionHelper.HasSphereBoxCollision(flippedCollisionInfo))
                {
                    return CollisionResult.LimitedCollision(flippedCollisionInfo);
                }
            }

            if (collisionInfo.AreShapes<Sphere, Polyhedron>())
            {
                if (collisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetSpherePolyhedronCollision(collisionInfo);
                }
                else if (CollisionHelper.HasSpherePolyhedronCollision(collisionInfo))
                {
                    return CollisionResult.LimitedCollision(collisionInfo);
                }
            }
            else if (collisionInfo.AreShapes<Polyhedron, Sphere>())
            {
                var flippedCollisionInfo = collisionInfo.Flipped();

                if (flippedCollisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetSpherePolyhedronCollision(flippedCollisionInfo);
                }
                else if (CollisionHelper.HasSpherePolyhedronCollision(flippedCollisionInfo))
                {
                    return CollisionResult.LimitedCollision(flippedCollisionInfo);
                }
            }

            if (collisionInfo.AreShapes<Box, Polyhedron>())
            {
                if (collisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetBoxPolyhedronCollision(collisionInfo);
                }
                else if (CollisionHelper.HasBoxPolyhedronCollision(collisionInfo))
                {
                    return CollisionResult.LimitedCollision(collisionInfo);
                }
            }
            else if (collisionInfo.AreShapes<Polyhedron, Box>())
            {
                var flippedCollisionInfo = collisionInfo.Flipped();

                if (flippedCollisionInfo.IsPhysical)
                {
                    return CollisionHelper.GetBoxPolyhedronCollision(flippedCollisionInfo);
                }
                else if (CollisionHelper.HasBoxPolyhedronCollision(flippedCollisionInfo))
                {
                    return CollisionResult.LimitedCollision(flippedCollisionInfo);
                }
            }

            return CollisionResult.NoCollision(collisionInfo);
        }

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

        public IEnumerable<CollisionResult> GetNarrowCollisions(int entityID)
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

        public void AddNarrowCollision(CollisionResult collisionResult)
        {
            var collisionPair = collisionResult.ConstructCollisionPair();

            _narrowCollisionByCollisionPair.Add(collisionPair, collisionResult);

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
