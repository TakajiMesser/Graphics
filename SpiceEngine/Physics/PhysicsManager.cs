using SpiceEngine.Entities;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Physics.Collisions;
using SpiceEngine.Physics.Constraints;
using SpiceEngine.Physics.Shapes;
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
        //private List<Tuple<int, Vector3>> _entityTranslations = new List<Tuple<int, Vector3>>();

        private Dictionary<int, Bounds> _boundsByEntityID = new Dictionary<int, Bounds>();
        private Dictionary<int, IBody> _bodyByEntityID = new Dictionary<int, IBody>();

        public int TickRate { get; set; } = 1;

        public PhysicsManager(IEntityProvider entityProvider, Quad worldBoundaries)
        {
            _entityProvider = entityProvider;

            _actorTree = new QuadTree(0, worldBoundaries);
            _brushTree = new QuadTree(0, worldBoundaries);
            _volumeTree = new QuadTree(0, worldBoundaries);
            _lightTree = new QuadTree(0, worldBoundaries);
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
            var shape = ((Body3D)_bodyByEntityID[entityID]).Shape;
            var entity = _entityProvider.GetEntity(entityID);
            var entityType = _entityProvider.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityTypes.Actor:
                case EntityTypes.Joint:
                    AddActor(entity, shape.Duplicate());
                    break;
                case EntityTypes.Brush:
                    AddBrush(entity, shape.Duplicate());
                    break;
                case EntityTypes.Volume:
                    AddVolume(entity, shape.Duplicate());
                    break;
            }
        }

        public void AddActor(IEntity entity, Shape3D shape)
        {
            var partition = shape.ToPartition(entity.Position);
            _actorTree.Insert(new Bounds(entity.ID, partition));

            var rigidBody = new RigidBody3D(entity, shape);
            rigidBody.ForceApplied += (s, args) => _bodiesToUpdate.Add(args.Body);
            rigidBody.Moved += (s, args) => entity.Position = args.Body.Position;
            _bodyByEntityID.Add(entity.ID, rigidBody);
        }

        public void AddBrush(IEntity entity, Shape3D shape)
        {
            var partition = shape.ToPartition(entity.Position);
            _brushTree.Insert(new Bounds(entity.ID, partition));

            var rigidBody = new StaticBody3D(entity, shape);
            _bodyByEntityID.Add(entity.ID, rigidBody);
        }

        public void AddVolume(IEntity entity, Shape3D shape)
        {
            var partition = shape.ToPartition(entity.Position);
            _volumeTree.Insert(new Bounds(entity.ID, partition));

            var rigidBody = new StaticBody3D(entity, shape);
            _bodyByEntityID.Add(entity.ID, rigidBody);
        }

        public IBody GetBody(int entityID)
        {
            if (_bodyByEntityID.ContainsKey(entityID))
            {
                return _bodyByEntityID[entityID];
            }

            // TODO - Will this ever be NULL? Should we throw an error instead?
            return null;
        }

        public IEnumerable<Collision3D> GetCollisions() => _collisionManager.NarrowCollisions;

        public IEnumerable<Collision3D> GetCollisions(int entityID) => _collisionManager.GetNarrowCollisions(entityID);

        public IEnumerable<int> GetCollisionIDs() => _collisionManager.NarrowCollisionIDs;

        public IEnumerable<int> GetCollisionIDs(int entityID) => _collisionManager.GetNarrowCollisionIDs(entityID);

        //public void ApplyForce(int entityID, Vector3 translation) => _entityTranslations.Add(Tuple.Create(entityID, translation));

        private HashSet<RigidBody3D> _bodiesToUpdate = new HashSet<RigidBody3D>();

        public void Update()
        {
            _collisionManager.Clear();

            BroadPhaseCollisionDetections();
            NarrowPhaseCollisionDetections();
            PerformCollisionResolutions();

            UpdatePositions();

            // The current order-of-operations is causing an undesirable outcome with gravity
            // First, we check for and mark any collisions
            // Next, we perform collision-resolutions:
                // if our actor is in the floor, this should cause an impulse in the positive Z direction
                // if our actor is in the physics volume, this should apply a force in the negative Z direction
            // Lastly, we update body positions
                // 
        }

        private void UpdatePositions()
        {
            var tickRate = TickRate;

            foreach (var body in _bodiesToUpdate)
            {
                body.Update(tickRate);
            }

            _bodiesToUpdate.Clear();
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
                var partition = ((Body3D)_bodyByEntityID[actor.ID]).Shape.ToPartition(actor.Position);
                var bounds = new Bounds(actor.ID, partition);

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
                        .Retrieve(bounds))
                    .Concat(_volumeTree
                        .Retrieve(bounds));

                _collisionManager.AddBroadCollision(actor.ID, colliderBounds.Select(b => b.EntityID));
            }
        }

        private void NarrowPhaseCollisionDetections()
        {
            // For each broad phase collision detection, check more narrowly to see if a collision actually did occur or not
            foreach (var collisionPair in _collisionManager.BroadCollisionPairs)
            {
                var firstBody = (Body3D)_bodyByEntityID[collisionPair.FirstEntityID];
                var secondBody = (Body3D)_bodyByEntityID[collisionPair.SecondEntityID];

                var firstEntity = _entityProvider.GetEntity(collisionPair.FirstEntityID);
                var secondEntity = _entityProvider.GetEntity(collisionPair.SecondEntityID);

                var collision = firstBody.GetCollision(secondBody);
                if (collision.HasCollision)
                {
                    _collisionManager.AddNarrowCollision(collision);
                }
            }
        }

        private void PerformCollisionResolutions()
        {
            foreach (var collision in _collisionManager.NarrowCollisions)
            {
                var firstEntitytype = _entityProvider.GetEntityType(collision.FirstBody.EntityID);
                var secondEntityType = _entityProvider.GetEntityType(collision.SecondBody.EntityID);
                var firstEntity = _entityProvider.GetEntity(collision.FirstBody.EntityID);
                var secondEntity = _entityProvider.GetEntity(collision.SecondBody.EntityID);

                // This is a pretty janky way to do things... this also assumes that a NarrowCollision must involve at least one RigidBody
                if (_entityProvider.GetEntityType(collision.FirstBody.EntityID) == EntityTypes.Volume && _entityProvider.GetEntity(collision.FirstBody.EntityID) is PhysicsVolume physicsVolume)
                {
                    if (collision.SecondBody is RigidBody3D rigidBody)
                    {
                        rigidBody.ApplyForce(physicsVolume.Gravity);
                    }
                }
                else if (_entityProvider.GetEntityType(collision.SecondBody.EntityID) == EntityTypes.Volume && _entityProvider.GetEntity(collision.SecondBody.EntityID) is PhysicsVolume physicsVolume2)
                {
                    if (collision.FirstBody is RigidBody3D rigidBody)
                    {
                        rigidBody.ApplyForce(physicsVolume2.Gravity);
                    }
                }
                else
                {
                    // For each collision that occurs, we need to determine what new forces (impulse) are going to get applied
                    PenetrationConstraint.Resolve(collision);
                }
            }
        }
    }
}
