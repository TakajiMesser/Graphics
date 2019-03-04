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

        private Dictionary<int, Bounds> _boundsByEntityID = new Dictionary<int, Bounds>();
        private Dictionary<int, IBody> _bodyByEntityID = new Dictionary<int, IBody>();
        private Dictionary<int, IBody> _awakeBodyByEntityID = new Dictionary<int, IBody>();

        private HashSet<RigidBody3D> _bodiesToUpdate = new HashSet<RigidBody3D>();

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

        public IEnumerable<Collision3D> GetCollisions() => _collisionManager.NarrowCollisions;

        public IEnumerable<Collision3D> GetCollisions(int entityID) => _collisionManager.GetNarrowCollisions(entityID);

        public IEnumerable<int> GetCollisionIDs() => _collisionManager.NarrowCollisionIDs;

        public IEnumerable<int> GetCollisionIDs(int entityID) => _collisionManager.GetNarrowCollisionIDs(entityID);

        public void DuplicateBody(int entityID, int newID)
        {
            var body = (Body3D)_bodyByEntityID[entityID];
            var shape = body.Shape;
            var entity = _entityProvider.GetEntity(entityID);

            switch (entity)
            {
                case Actor actor:
                    AddActor(actor, shape, body.IsPhysical);
                    break;
                case Brush brush:
                    AddBrush(brush, shape);
                    break;
                case Volume volume:
                    AddVolume(volume, shape);
                    break;
            }
        }

        public void AddActor(Actor actor, Shape3D shape, bool isPhysical)
        {
            var partition = shape.ToPartition(actor.Position);
            _actorTree.Insert(new Bounds(actor.ID, partition));

            var body = new RigidBody3D(actor, shape)
            {
                IsPhysical = isPhysical
            };
            body.ForceApplied += (s, args) => _bodiesToUpdate.Add(args.Body);
            body.Moved += (s, args) => entity.Position = args.Body.Position;
            _bodyByEntityID.Add(actor.ID, body);

            if (body.State == BodyStates.Awake)
            {
                _awakeBodyByEntityID.Add(actor.ID, body);
            }
        }

        public void AddBrush(Brush brush, Shape3D shape)
        {
            var partition = shape.ToPartition(brush.Position);
            _brushTree.Insert(new Bounds(brush.ID, partition));

            var body = new StaticBody3D(brush, shape);
            _bodyByEntityID.Add(brush.ID, body);
        }

        public void AddVolume(Volume volume, Shape3D shape)
        {
            var partition = shape.ToPartition(volume.Position);
            _volumeTree.Insert(new Bounds(volume.ID, partition));

            var body = new StaticBody3D(volume, shape)
            {
                IsPhysical = volume is BlockingVolume
            };

            _bodyByEntityID.Add(volume.ID, body);
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

        public void Update()
        {
            _collisionManager.Clear();

            BroadPhaseCollisionDetections();
            NarrowPhaseCollisionDetections();
            PerformCollisionResolutions();

            UpdatePositions();
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
                // Only resolve the penetration constraint if both bodies are physical. Resolve by determining and applying impulses to each body
                if (collision.FirstBody.IsPhysical && collision.SecondBody.IsPhysical)
                {
                    PenetrationConstraint.Resolve(collision);
                }

                // If one of the bodies is a PhysicsVolume, we need to apply its gravity
                ResolvePhysicsVolume(collision.FirstBody, collision.SecondBody);

                // If one of the bodies is a TriggerVolume, we need to trigger it. If the actor has a contact, proximity, or sight response, we need to trigger it.
                ResolveTriggerVolume(collision.FirstBody, collision.SecondBody);
            }
        }

        private void ResolvePhysicsVolume(Body3D bodyA, Body3D bodyB)
        {
            var entityA = _entityProvider.GetEntity(bodyA.EntityID);
            var entityB = _entityProvider.GetEntity(bodyB.EntityID);

            if (entityA is PhysicsVolume)
            {
                var physicsVolume = (PhysicsVolume)entityA;

                if (entityB is RigidBody3D rigidBody)
                {
                    rigidBody.ApplyForce(physicsVolume.Gravity);
                }
            }
            else if (entityB is PhysicsVolume)
            {
                var physicsVolume = (physicsVolume)entityB;

                if (entityA is RigidBody3D rigidBody)
                {
                    rigidBody.ApplyForce(physicsVolume.Gravity);
                }
            }
        }

        private void ResolveTriggerVolume(Body3D bodyA, Body3D bodyB)
        {
            var entityA = _entityProvider.GetEntity(bodyA.EntityID);
            var entityB = _entityProvider.GetEntity(bodyB.EntityID);

            if (entityA is TriggerVolume)
            {
                var triggerVolume = (TriggerVolume)entityA;
                triggerVolume.OnTriggered(entityA);
            }
            else if (entityB is TriggerVolume)
            {
                var triggerVolume = (TriggerVolume)entityB;
                triggerVolume.OnTriggered(entityB);
            }
        }
    }
}
