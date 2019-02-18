using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Physics
{
    public class PhysicsManager : ICollisionProvider
    {
        private IEntityProvider _entityProvider;

        private IPartitionTree _actorTree;
        private IPartitionTree _brushTree;
        private IPartitionTree _volumeTree;
        private IPartitionTree _lightTree;

        private CollisionManager _collisionManager = new CollisionManager();
        private List<Tuple<int, Vector3>> _entityTranslations = new List<Tuple<int, Vector3>>();

        private Dictionary<int, Bounds> _boundsByEntityID = new Dictionary<int, Bounds>();
        private Dictionary<int, Body> _bodyByEntityID = new Dictionary<int, Body>();

        public List<EntityCollision> EntityCollisions { get; } = new List<EntityCollision>();
        
        public PhysicsManager(IEntityProvider entityProvider, Quad worldBoundaries)
        {
            _entityProvider = entityProvider;

            _actorTree = new QuadTree(0, worldBoundaries);
            _brushTree = new QuadTree(0, worldBoundaries);
            _volumeTree = new QuadTree(0, worldBoundaries);
            _lightTree = new QuadTree(0, worldBoundaries);

            /*_lightQuads = new QuadTree(0, map.Boundaries);
            _lightQuads.InsertRange(EntityManager.Lights.Select(l => new BoundingCircle(l)));

            _brushQuads = new QuadTree(0, map.Boundaries);
            _brushQuads.InsertRange(EntityManager.Brushes.Where(b => b.HasCollision).Select(b => b.Bounds));

            _volumeQuads = new QuadTree(0, map.Boundaries);
            _volumeQuads.InsertRange(EntityManager.Volumes.Select(v => v.Bounds));

            _actorQuads = new QuadTree(0, map.Boundaries);*/
        }

        public PhysicsManager(IEntityProvider entityProvider, Oct worldBoundaries)
        {
            _entityProvider = entityProvider;

            _actorTree = new OctTree(0, worldBoundaries);
            _brushTree = new OctTree(0, worldBoundaries);
            _volumeTree = new OctTree(0, worldBoundaries);
            _lightTree = new OctTree(0, worldBoundaries);
        }

        public void DuplicateBody(int entityID, int newID)
        {
            var position = _entityProvider.GetEntity(entityID).Position;
            var shape = _bodyByEntityID[entityID].Shape;
            var entityType = _entityProvider.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityTypes.Actor:
                case EntityTypes.Joint:
                    AddActor(newID, shape.Duplicate(), position);
                    break;
                case EntityTypes.Brush:
                    AddBrush(newID, shape.Duplicate(), position);
                    break;
                case EntityTypes.Volume:
                    AddVolume(newID, shape.Duplicate(), position);
                    break;
            }
        }

        public void AddBrush(int entityID, IShape shape, Vector3 position)
        {
            var collider = shape.ToCollider(position);
            _brushTree.Insert(new Bounds(entityID, collider));

            var rigidBody = new RigidBody(entityID, shape);
            _bodyByEntityID.Add(entityID, rigidBody);
        }

        public void AddActor(int entityID, IShape shape, Vector3 position)
        {
            var collider = shape.ToCollider(position);
            _actorTree.Insert(new Bounds(entityID, collider));

            var rigidBody = new RigidBody(entityID, shape);
            _bodyByEntityID.Add(entityID, rigidBody);
        }

        public void AddVolume(int entityID, IShape shape, Vector3 position)
        {
            var collider = shape.ToCollider(position);
            _volumeTree.Insert(new Bounds(entityID, collider));

            var rigidBody = new RigidBody(entityID, shape);
            _bodyByEntityID.Add(entityID, rigidBody);
        }

        public Body GetBody(int entityID)
        {
            if (_bodyByEntityID.ContainsKey(entityID))
            {
                return _bodyByEntityID[entityID];
            }

            // TODO - Will this ever be NULL? Should we throw an error instead?
            return null;
        }

        public IEnumerable<Collision> GetCollisions() => _collisionManager.NarrowCollisions;

        public IEnumerable<Collision> GetCollisions(int entityID) => _collisionManager.GetNarrowCollisions(entityID);

        public IEnumerable<int> GetCollisionIDs() => _collisionManager.NarrowCollisionIDs;

        public IEnumerable<int> GetCollisionIDs(int entityID) => _collisionManager.GetNarrowCollisionIDs(entityID);

        public void ApplyForce(int entityID, Vector3 translation) => _entityTranslations.Add(Tuple.Create(entityID, translation));

        public void Update()
        {
            ApplyForces();

            // TODO - Determine order of operations here
            // The issue is that after applying forces, the positions will move, so we need to perform CD and CR
            // HOWEVER, when we perform the behaviors/scripts for Actors, the positions can potentially move again!
            // Does this mean that we perform CD and CR again? Sounds pretty inefficient...
            // Maybe we can just have the behaviors/scripts affect the positions, BUT we don't perform CD and CR again until the next frame!
            _collisionManager.Clear();
            BroadPhaseCollisionDetections();
            NarrowPhaseCollisionDetections();
            PerformCollisionResolutions();
        }

        private void ApplyForces()
        {
            // TODO - For all bodies, calculate their new velocities and positions given their forces
            // For now, we are just using very basic translations passed each frame for each entity
            // We will also want this step of applying the force separated from the step of resolving the collision
            // For now, we are performing these two steps together, which is BAD because it defeats the whole point of doing CD
            // We want to apply the forces, get the new position, then when we do CR, and use the linear velocity to know where the entity moved from
            // From there, we can either A) Just revert back to the previous position by using the linear velocity, or (better)
            // B) Apply an appropriate force to send the object in its new direction
            // Maybe we can use the linear velocity to determine the time delta of collision, then base the new position off of that
            foreach (var entityTranslation in _entityTranslations)
            {
                var actor = _entityProvider.GetEntity(entityTranslation.Item1);
                var shape = (Shape3D)_bodyByEntityID[actor.ID].Shape;

                Vector3 translation = entityTranslation.Item2;

                if (shape != null)
                {
                    foreach (var body in _bodyByEntityID.Values.Where(b => b.EntityID != actor.ID))
                    {
                        var colliderPosition = _entityProvider.GetEntity(body.EntityID).Position;

                        if (Shape3D.Collides(new Vector3(actor.Position.X + translation.X, actor.Position.Y, actor.Position.Z), shape, colliderPosition, (Shape3D)body.Shape))
                        {
                            translation.X = 0;
                        }

                        if (Shape3D.Collides(new Vector3(actor.Position.X, actor.Position.Y + translation.Y, actor.Position.Z), shape, colliderPosition, (Shape3D)body.Shape))
                        {
                            translation.Y = 0;
                        }

                        if (Shape3D.Collides(new Vector3(actor.Position.X, actor.Position.Y, actor.Position.Z + translation.Z), shape, colliderPosition, (Shape3D)body.Shape))
                        {
                            translation.Z = 0;
                        }
                    }
                }

                actor.Position += translation;
            }

            _entityTranslations.Clear();
        }

        private void BroadPhaseCollisionDetections()
        {
            // We just need to perform these collision detections for Actors, since they are all that can move
            // Can Volumes move? Lights? Maybe Lights need to be attached to an Actor in order to move?
            _actorTree.Clear();
            var boundsByEntityID = new Dictionary<int, Bounds>();

            // Update the actor colliders every frame, since they could have moved
            foreach (var actor in _entityProvider.Actors)
            {
                var collider = _bodyByEntityID[actor.ID].Shape.ToCollider(actor.Position);
                var bounds = new Bounds(actor.ID, collider);

                boundsByEntityID.Add(actor.ID, bounds);
                _actorTree.Insert(bounds);
            }

            // Now, for each actor, check for broad collisions against other actors, brushes, and volumes
            foreach (var actor in _entityProvider.Actors)
            {
                var bounds = boundsByEntityID[actor.ID];

                var colliderBounds = _actorTree.Retrieve(bounds)
                    .Where(b => b.EntityID != actor.ID)
                    .Concat(_brushTree
                        .Retrieve(bounds));

                _collisionManager.AddBroadCollision(actor.ID, colliderBounds.Select(b => b.EntityID));
            }
        }

        private void NarrowPhaseCollisionDetections()
        {
            // For each broad phase collision detection, check more narrowly to see if a collision actually did occur or not
            foreach (var collisionPair in _collisionManager.BroadCollisionPairs)
            {
                var firstBody = _bodyByEntityID[collisionPair.FirstEntityID];
                var secondBody = _bodyByEntityID[collisionPair.SecondEntityID];

                var firstEntity = _entityProvider.GetEntity(collisionPair.FirstEntityID);
                var secondEntity = _entityProvider.GetEntity(collisionPair.SecondEntityID);

                if (Shape3D.Collides(firstEntity.Position, (Shape3D)firstBody.Shape, secondEntity.Position, (Shape3D)secondBody.Shape))
                {
                    var collision = new Collision(collisionPair);
                    _collisionManager.AddNarrowCollision(collision);
                }
            }
        }

        private void PerformCollisionResolutions()
        {
            foreach (var collision in _collisionManager.NarrowCollisions)
            {
                // For each collision pair, 
            }
        }
    }
}
