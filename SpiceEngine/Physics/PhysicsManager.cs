using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Volumes;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using UmamiPhysicsCore.Bodies;
using UmamiPhysicsCore.Collisions;
using UmamiPhysicsCore.Constraints;
using UmamiPhysicsCore.Shapes;

namespace SpiceEngine.Physics
{
    public class PhysicsManager : ComponentLoader<IShape, IShapeBuilder>, ICollisionProvider
    {
        private IPartitionTree _actorTree;
        private IPartitionTree _brushTree;
        private IPartitionTree _volumeTree;
        private IPartitionTree _lightTree;

        private CollisionManager _collisionManager = new CollisionManager();

        private Dictionary<int, IBody> _bodyByEntityID = new Dictionary<int, IBody>();
        //private Dictionary<int, Bounds> _boundsByEntityID = new Dictionary<int, Bounds>();
        private Dictionary<int, IBody> _awakeBodyByEntityID = new Dictionary<int, IBody>();
        //private Dictionary<int, IBody> _bodyByEntityID = new Dictionary<int, IBody>();
        //private ConcurrentDictionary<int, IBody> _bodyByEntityID = new ConcurrentDictionary<int, IBody>();

        private HashSet<RigidBody3D> _bodiesToUpdate = new HashSet<RigidBody3D>();

        public PhysicsManager(IEntityProvider entityProvider, Quad worldBoundaries)
        {
            SetEntityProvider(entityProvider);

            _actorTree = new QuadTree(0, worldBoundaries);
            _brushTree = new QuadTree(0, worldBoundaries);
            _volumeTree = new QuadTree(0, worldBoundaries);
            _lightTree = new QuadTree(0, worldBoundaries);
        }

        public PhysicsManager(IEntityProvider entityProvider, Oct worldBoundaries)
        {
            SetEntityProvider(entityProvider);

            _actorTree = new OctTree(0, worldBoundaries);
            _brushTree = new OctTree(0, worldBoundaries);
            _volumeTree = new OctTree(0, worldBoundaries);
            _lightTree = new OctTree(0, worldBoundaries);
        }

        public IEnumerable<ICollision> GetCollisions() => _collisionManager.NarrowCollisions;

        public IEnumerable<ICollision> GetCollisions(int entityID) => _collisionManager.GetNarrowCollisions(entityID);

        public IEnumerable<int> GetCollisionIDs() => _collisionManager.NarrowCollisionIDs;

        public IEnumerable<int> GetCollisionIDs(int entityID) => _collisionManager.GetNarrowCollisionIDs(entityID);

        /*public override Task LoadBuilderAsync(int entityID, IShapeBuilder builder) => Task.Run(() =>
        {
            var component = builder.ToComponent();

            if (component != null)
            {
                var partition = component.ToPartition(builder.Position);
                var bounds = new Bounds(entityID, partition);

                // TODO - This incorrectly relies on the entity provider to have already inserted this entity
                var partitionTree = GetPartitionTree(entityID);
                partitionTree.Insert(bounds);

                // TODO - So does this :/
                var body = GetBody(entityID, component);
                if (body != null)
                {
                    body.IsPhysical = builder.IsPhysical;
                }
            }
        });*/

        private Dictionary<int, Vector3> _initialPositionByID = new Dictionary<int, Vector3>();
        private Dictionary<int, bool> _isPhysicalByID = new Dictionary<int, bool>();

        public override void LoadBuilderSync(int entityID, IShapeBuilder builder)
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
        protected override void LoadComponent(int entityID, IShape component)
        {
            base.LoadComponent(entityID, component);

            var partition = component.ToPartition(_initialPositionByID[entityID]);
            var bounds = new Bounds(entityID, partition);

            // TODO - This incorrectly relies on the entity provider to have already inserted this entity
            var partitionTree = GetPartitionTree(entityID);
            partitionTree.Insert(bounds);

            // TODO - So does this :/
            var body = GetBody(entityID, component);
            if (body != null)
            {
                body.IsPhysical = _isPhysicalByID[entityID];
                _bodyByEntityID.Add(entityID, body);
            }
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
            var entity = _entityProvider.GetEntity(entityID);

            switch (entity)
            {
                case IActor actor:
                    return _actorTree;
                case IBrush brush:
                    return _brushTree;
                case IVolume volume:
                    return _volumeTree;
                case ILight light:
                    return _lightTree;
            }

            throw new ArgumentOutOfRangeException("Could not handle entity type " + entity.GetType());
        }

        private IBody GetBody(int entityID, IShape shape)
        {
            var entity = _entityProvider.GetEntity(entityID);

            if (shape is Shape3D shape3D)
            {
                switch (entity)
                {
                    case IActor actor:
                        var body = new RigidBody3D(actor, shape3D);
                        body.Influenced += (s, args) => _bodiesToUpdate.Add(args.Body);
                        body.Updated += (s, args) => actor.Position = args.Body.Position;

                        if (body.State == BodyStates.Awake)
                        {
                            _awakeBodyByEntityID.Add(entityID, body);
                        }

                        return body;
                    case IBrush brush:
                        return new StaticBody3D(brush, shape3D);
                    case IVolume volume:
                        return new StaticBody3D(volume, shape3D);
                    case ILight light:
                        return null;
                }
            }

            throw new ArgumentOutOfRangeException("Could not handle entity type " + entity.GetType());
        } 

        public void DuplicateBody(int entityID, int newID)
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
            }*/
        }

        /*public void AddActor(Actor actor, Shape3D shape, bool isPhysical)
        {
            var partition = shape.ToPartition(actor.Position);
            _actorTree.Insert(new Bounds(actor.ID, partition));

            var body = new RigidBody3D(actor, shape)
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

        public IBody GetBody(int entityID)
        {
            if (_bodyByEntityID.TryGetValue(entityID, out IBody body))
            {
                return body;
            }
            else
            {
                return null;
            }
        }

        protected override void Update()
        {
            _collisionManager.Clear();

            BroadPhaseCollisionDetections();
            NarrowPhaseCollisionDetections();
            PerformCollisionResolutions();
            UpdatePositions();
        }

        private void BroadPhaseCollisionDetections()
        {
            // We just need to perform these collision detections for Actors, since they are all that can move
            // Can Volumes move? Lights? Maybe Lights need to be attached to an Actor in order to move?
            _actorTree.Clear();
            var boundsByEntityID = new Dictionary<int, Bounds>();

            // TODO - ONLY move the actor colliders that reported movement in the last frame (including physics)
            // Update the actor colliders every frame, since they could have moved
            foreach (var actor in _entityProvider.Actors)
            {
                if (_componentByID.ContainsKey(actor.ID))
                {
                    var shape = _componentByID[actor.ID];
                    var partition = shape.ToPartition(actor.Position);
                    var bounds = new Bounds(actor.ID, partition);

                    boundsByEntityID.Add(actor.ID, bounds);
                    _actorTree.Insert(bounds);

                }
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
                if (_bodyByEntityID.TryGetValue(collisionPair.FirstEntityID, out IBody firstBody) && firstBody is Body3D bodyA
                    && _bodyByEntityID.TryGetValue(collisionPair.SecondEntityID, out IBody secondBody) && secondBody is Body3D bodyB)
                {
                    var entityA = _entityProvider.GetEntity(collisionPair.FirstEntityID);
                    var entityB = _entityProvider.GetEntity(collisionPair.SecondEntityID);

                    var collision = bodyA.GetCollision(bodyB);
                    if (collision.HasCollision && collision is Collision3D collision3D)
                    {
                        _collisionManager.AddNarrowCollision(collision3D);
                    }
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

                if (bodyB is RigidBody3D rigidBody)
                {
                    rigidBody.ApplyForce(physicsVolume.Gravity);
                }
            }
            else if (entityB is PhysicsVolume)
            {
                var physicsVolume = (PhysicsVolume)entityB;

                if (bodyA is RigidBody3D rigidBody)
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

        private void UpdatePositions()
        {
            var tickRate = TickRate;

            foreach (var body in _bodiesToUpdate)
            {
                body.Update(tickRate);
            }

            _bodiesToUpdate.Clear();
        }
    }
}
