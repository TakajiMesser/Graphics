using SavoryPhysicsCore.Bodies;
using SavoryPhysicsCore.Collisions;
using SavoryPhysicsCore.Constraints;
using SavoryPhysicsCore.Partitioning;
using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Volumes;
using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SavoryPhysicsCore
{
    public class PhysicsSystem : ComponentSystem<IBody, IBodyBuilder>, IPhysicsProvider
    {
        private IPartitionTree _actorTree;
        private IPartitionTree _brushTree;
        private IPartitionTree _volumeTree;
        private IPartitionTree _lightTree;

        private CollisionManager _collisionManager = new CollisionManager();

        //private Dictionary<int, IBody> _bodyByEntityID = new Dictionary<int, IBody>();
        //private Dictionary<int, Bounds> _boundsByEntityID = new Dictionary<int, Bounds>();
        //private Dictionary<int, IBody> _awakeBodyByEntityID = new Dictionary<int, IBody>();
        //private Dictionary<int, IBody> _bodyByEntityID = new Dictionary<int, IBody>();
        //private ConcurrentDictionary<int, IBody> _bodyByEntityID = new ConcurrentDictionary<int, IBody>();

        private HashSet<int> _awakeBodyIDs = new HashSet<int>();
        private HashSet<IBody> _bodiesToUpdate = new HashSet<IBody>();

        private Dictionary<int, Vector3> _initialPositionByID = new Dictionary<int, Vector3>();
        private Dictionary<int, bool> _isPhysicalByID = new Dictionary<int, bool>();

        public void SetBoundaries(IPartition boundaries)
        {
            switch (boundaries)
            {
                case Oct oct:
                    _actorTree = new OctTree(0, oct);
                    _brushTree = new OctTree(0, oct);
                    _volumeTree = new OctTree(0, oct);
                    _lightTree = new OctTree(0, oct);
                    break;
                case Quad quad:
                    _actorTree = new QuadTree(0, quad);
                    _brushTree = new QuadTree(0, quad);
                    _volumeTree = new QuadTree(0, quad);
                    _lightTree = new QuadTree(0, quad);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Could not handle partition " + boundaries);
            }
        }

        public IEnumerable<CollisionResult> GetCollisions() => _collisionManager.NarrowCollisions;

        public IEnumerable<CollisionResult> GetCollisions(int entityID) => _collisionManager.GetNarrowCollisions(entityID);

        public IEnumerable<int> GetCollisionIDs() => _collisionManager.NarrowCollisionIDs;

        public IEnumerable<int> GetCollisionIDs(int entityID) => _collisionManager.GetNarrowCollisionIDs(entityID);

        public IEnumerable<IBody> GetCollisionBodies(int entityID) => _collisionManager.GetNarrowCollisionBodies(entityID);

        public override void LoadBuilderSync(int entityID, IBodyBuilder builder)
        {
            base.LoadBuilderSync(entityID, builder);

            _initialPositionByID.Add(entityID, builder.Position);
            _isPhysicalByID.Add(entityID, builder.IsPhysical);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();

            _initialPositionByID.Clear();
            _isPhysicalByID.Clear();
        }

        // TODO - Set up component loaders to rely on this entity being loaded in the EntityProvider by the time this method is invoked (not currently guaranteed)
        protected override void LoadComponent(IBody component)
        {
            base.LoadComponent(component);

            var position = _initialPositionByID[component.EntityID];
            var partition = component.Shape.ToPartition(position);
            var bounds = new Bounds(component.EntityID, partition);

            var partitionTree = GetPartitionTree(component.EntityID);
            partitionTree.Insert(bounds);

            var entity = _systemProvider.GetEntity(component.EntityID);
            if (component is RigidBody rigidBody)
            {
                rigidBody.Influenced += (s, args) => _bodiesToUpdate.Add(args.Body);
                //rigidBody.Updated += (s, args) => entity.Position = args.Body.;

                if (component.State == BodyStates.Awake)
                {
                    _awakeBodyIDs.Add(component.EntityID);
                }
            }

            /*var body = GetComponentOrDefault(component.EntityID, component);
            if (body != null)
            {
                body.IsPhysical = _isPhysicalByID[component.EntityID];
                _bodyByEntityID.Add(component.EntityID, body);
            }*/

            component.IsPhysical = _isPhysicalByID[component.EntityID];
            //_componentByID.Add(component.EntityID, component);
        }

        /*public void AddComponent(int entityID, IShapeBuilder builder)
        {
            var shape = builder.ToComponent();
            var partition = shape.ToPartition(builder.Position);
            var bounds = new Bounds(entityID, partition);

            var partitionTree = GetPartitionTree(entityID);
            partitionTree.Insert(bounds);

            var body = GetBody(entityID, shape);
            if (body != null)
            {
                body.IsPhysical = builder.IsPhysical;
                _bodyByEntityID.TryAdd(entityID, body);
            }
        }*/

        private IPartitionTree GetPartitionTree(int entityID)
        {
            var entity = _systemProvider.GetEntity(entityID);

            switch (entity)
            {
                case IActor _:
                    return _actorTree;
                case IBrush _:
                    return _brushTree;
                case IVolume _:
                    return _volumeTree;
                case ILight _:
                    return _lightTree;
            }

            throw new ArgumentOutOfRangeException("Could not handle entity type " + entity.GetType());
        }

        /*public void DuplicateBody(int entityID, int newID)
        {
            var body = (Body3D)_bodyByEntityID[entityID];
            var shape = body.Shape;
            var entity = _entityProvider.GetEntity(entityID);

            /*switch (entity)
            {
                case Actor actor:
                    AddActor(actor, shape, body.IsPhysical);
                    break;
                case Brush brush:
                    AddBrush(brush, shape, body.IsPhysical);
                    break;
                case Volume volume:
                    AddVolume(volume, shape);
                    break;
            }*
        }*/

        /*public void AddActor(Actor actor, Shape3D shape, bool isPhysical)
        {
            var partition = shape.ToPartition(actor.Position);
            _actorTree.Insert(new Bounds(actor.ID, partition));

            var body = new RigidBody(actor, shape)
            {
                IsPhysical = isPhysical
            };
            body.Influenced += (s, args) => _bodiesToUpdate.Add(args.Body);
            body.Updated += (s, args) => actor.Position = args.Body.Position;
            _bodyByEntityID.Add(actor.ID, body);

            if (body.State == BodyStates.Awake)
            {
                _awakeBodyByEntityID.Add(actor.ID, body);
            }
        }

        public void AddBrush(Brush brush, Shape3D shape, bool isPhysical)
        {
            var partition = shape.ToPartition(brush.Position);
            _brushTree.Insert(new Bounds(brush.ID, partition));

            var body = new StaticBody3D(brush, shape)
            {
                IsPhysical = isPhysical
            };
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
        }*/

        protected override void Update()
        {
            _collisionManager.Clear();

            BroadPhaseCollisionDetections();
            NarrowPhaseCollisionDetections();
            PerformCollisionResolutions();
            UpdateTransforms();
        }

        private void BroadPhaseCollisionDetections()
        {
            // We just need to perform these collision detections for Actors, since they are all that can move
            // Can Volumes move? Lights? Maybe Lights need to be attached to an Actor in order to move?
            _actorTree.Clear();
            var boundsByEntityID = new Dictionary<int, Bounds>();

            // TODO - ONLY move the actor colliders that reported movement in the last frame (including physics)
            // Update the actor colliders every frame, since they could have moved
            foreach (var actor in _systemProvider.EntityProvider.Actors)
            {
                if (_componentByID.ContainsKey(actor.ID))
                {
                    var body = _componentByID[actor.ID];
                    var partition = body.Shape.ToPartition(actor.Position);
                    var bounds = new Bounds(actor.ID, partition);

                    boundsByEntityID.Add(actor.ID, bounds);
                    _actorTree.Insert(bounds);
                }
            }

            // Now, for each actor, check for broad collisions against other actors, brushes, and volumes
            foreach (var actor in _systemProvider.EntityProvider.Actors)
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
                var bodyA = _componentByID[collisionPair.FirstEntityID];
                var bodyB = _componentByID[collisionPair.SecondEntityID];

                var entityA = _systemProvider.GetEntity(collisionPair.FirstEntityID);
                var entityB = _systemProvider.GetEntity(collisionPair.SecondEntityID);

                var collisionInfo = new CollisionInfo(entityA, entityB, bodyA, bodyB);
                var collisionResult = _collisionManager.PerformCollisionCheck(collisionInfo);

                if (collisionResult.HasCollision)
                {
                    _collisionManager.AddNarrowCollision(collisionResult);
                }
            }
        }

        private void PerformCollisionResolutions()
        {
            foreach (var collision in _collisionManager.NarrowCollisions)
            {
                // Only resolve the penetration constraint if both bodies are physical. Resolve by determining and applying impulses to each body
                if (collision.CollisionInfo.IsPhysical)
                {
                    PenetrationConstraint.Resolve(collision);
                }

                // If one of the bodies is a PhysicsVolume, we need to apply its gravity
                ResolvePhysicsVolume(collision.CollisionInfo.BodyA, collision.CollisionInfo.BodyB);

                // If one of the bodies is a TriggerVolume, we need to trigger it. If the actor has a contact, proximity, or sight response, we need to trigger it.
                ResolveTriggerVolume(collision.CollisionInfo.BodyA, collision.CollisionInfo.BodyB);
            }
        }

        private void ResolvePhysicsVolume(IBody bodyA, IBody bodyB)
        {
            var entityA = _systemProvider.GetEntity(bodyA.EntityID);
            var entityB = _systemProvider.GetEntity(bodyB.EntityID);

            if (entityA is PhysicsVolume)
            {
                var physicsVolume = (PhysicsVolume)entityA;

                if (bodyB is RigidBody rigidBody)
                {
                    rigidBody.ApplyForce(physicsVolume.Gravity);
                }
            }
            else if (entityB is PhysicsVolume)
            {
                var physicsVolume = (PhysicsVolume)entityB;

                if (bodyA is RigidBody rigidBody)
                {
                    rigidBody.ApplyForce(physicsVolume.Gravity);
                }
            }
        }

        private void ResolveTriggerVolume(IBody bodyA, IBody bodyB)
        {
            var entityA = _systemProvider.GetEntity(bodyA.EntityID);
            var entityB = _systemProvider.GetEntity(bodyB.EntityID);

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

        private void UpdateTransforms()
        {
            var tickRate = TickRate;

            foreach (var body in _bodiesToUpdate)
            {
                var entity = _systemProvider.GetEntity(body.EntityID);
                entity.Position = body.UpdatePosition(entity.Position, tickRate);

                if (entity is IRotate rotator)
                {
                    rotator.Rotation = body.UpdateRotation(rotator.Rotation, tickRate);
                }
            }

            _bodiesToUpdate.Clear();
        }
    }
}
