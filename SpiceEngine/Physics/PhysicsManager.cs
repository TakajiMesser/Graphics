using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Physics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics
{
    public class PhysicsManager
    {
        private IEntityProvider _entityProvider;
        private ITranslationProvider _translationProvider;

        private ICollisionTree _actorTree;
        private ICollisionTree _brushTree;
        private ICollisionTree _volumeTree;
        private ICollisionTree _lightTree;

        private Dictionary<int, RigidBody> _rigidBodyByEntityID = new Dictionary<int, RigidBody>();

        public List<EntityCollision> EntityCollisions { get; } = new List<EntityCollision>();
        
        public PhysicsManager(IEntityProvider entityProvider, ITranslationProvider translationProvider, Quad worldBoundaries)
        {
            _entityProvider = entityProvider;
            _translationProvider = translationProvider;

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

        public PhysicsManager(IEntityProvider entityProvider, ITranslationProvider translationProvider, Oct worldBoundaries)
        {
            _entityProvider = entityProvider;
            _translationProvider = translationProvider;

            _actorTree = new OctTree(0, worldBoundaries);
            _brushTree = new OctTree(0, worldBoundaries);
            _volumeTree = new OctTree(0, worldBoundaries);
            _lightTree = new OctTree(0, worldBoundaries);
        }

        public void DuplicateBody(int entityID, int newID)
        {
            var position = _entityProvider.GetEntity(entityID).Position;
            var shape = _rigidBodyByEntityID[entityID].Shape;
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
            _rigidBodyByEntityID.Add(entityID, rigidBody);
        }

        public void AddActor(int entityID, IShape shape, Vector3 position)
        {
            var collider = shape.ToCollider(position);
            _actorTree.Insert(new Bounds(entityID, collider));

            var rigidBody = new RigidBody(entityID, shape);
            _rigidBodyByEntityID.Add(entityID, rigidBody);
        }

        public void AddVolume(int entityID, IShape shape, Vector3 position)
        {
            var collider = shape.ToCollider(position);
            _volumeTree.Insert(new Bounds(entityID, collider));

            var rigidBody = new RigidBody(entityID, shape);
            _rigidBodyByEntityID.Add(entityID, rigidBody);
        }

        public void Update()
        {
            // Update the actor colliders every frame, since they could have moved
            _actorTree.Clear();
            EntityCollisions.Clear();
            var boundsByID = new Dictionary<int, Bounds>();

            foreach (var actor in _entityProvider.Actors)
            {
                var shape = _rigidBodyByEntityID[actor.ID].Shape;
                var collider = shape.ToCollider(actor.Position);
                var bounds = new Bounds(actor.ID, collider);

                boundsByID.Add(actor.ID, bounds);
                _actorTree.Insert(bounds);
            }

            // For each object that has a non-zero transform, we need to determine the set of colliders to compare it against for hit detection
            foreach (var actor in _entityProvider.Actors)
            {
                //actor.ClearLights();
                //actor.AddPointLights(_lightQuads.Retrieve(actor.Bounds)
                //    .Where(c => c.AttachedEntity is PointLight)
                //    .Select(c => (PointLight)c.AttachedEntity));
                var bounds = boundsByID[actor.ID];

                var filteredColliders = _brushTree.Retrieve(bounds)
                    .Concat(_actorTree
                        .Retrieve(bounds)
                        .Where(b => b.EntityID != actor.ID));

                EntityCollisions.Add(new EntityCollision(actor.ID)
                {
                    Shape = _rigidBodyByEntityID[actor.ID].Shape,
                    Bounds = bounds,
                    Colliders = filteredColliders,
                    Bodies = filteredColliders.Select(c => _rigidBodyByEntityID[c.EntityID])
                });
            }
        }

        public virtual void HandleActorCollisions()
        {
            foreach (var entityTranslation in _translationProvider.EntityTranslations)
            {
                var actor = _entityProvider.GetEntity(entityTranslation.EntityID);

                Vector3 translation = entityTranslation.Translation;
                var physics = EntityCollisions.FirstOrDefault(p => p.EntityID == entityTranslation.EntityID);

                if (physics.Shape != null)
                {
                    foreach (var collider in physics.Bodies)
                    {
                        var colliderPosition = _entityProvider.GetEntity(collider.EntityID).Position;

                        if (Shape3D.Collides(new Vector3(actor.Position.X + translation.X, actor.Position.Y, actor.Position.Z), (Shape3D)physics.Shape, colliderPosition, (Shape3D)collider.Shape))
                        {
                            translation.X = 0;
                        }

                        if (Shape3D.Collides(new Vector3(actor.Position.X, actor.Position.Y + translation.Y, actor.Position.Z), (Shape3D)physics.Shape, colliderPosition, (Shape3D)collider.Shape))
                        {
                            translation.Y = 0;
                        }

                        if (Shape3D.Collides(new Vector3(actor.Position.X, actor.Position.Y, actor.Position.Z + translation.Z), (Shape3D)physics.Shape, colliderPosition, (Shape3D)collider.Shape))
                        {
                            translation.Z = 0;
                        }
                    }
                }

                actor.Position += translation;
            }
        }
    }
}
