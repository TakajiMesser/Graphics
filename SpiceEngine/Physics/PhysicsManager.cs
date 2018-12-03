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

        private ICollisionTree _actorTree;
        private ICollisionTree _brushTree;
        private ICollisionTree _volumeTree;
        private ICollisionTree _lightTree;

        private Dictionary<int, RigidBody> _rigidBodyByEntityID = new Dictionary<int, RigidBody>();

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

                UpdateActor(actor, bounds, filteredColliders);
            }
        }

        public void UpdateActor(Actor actor, Bounds bounds, IEnumerable<Bounds> colliders)
        {
            if (actor.Behaviors != null)
            {
                var shape = _rigidBodyByEntityID[actor.ID].Shape;

                //Behaviors.Context.Rotation = Rotation;
                actor.Behaviors.Context.EntityProvider = _entityProvider;
                actor.Behaviors.Context.ActorShape = shape;
                actor.Behaviors.Context.ActorBounds = bounds;
                actor.Behaviors.Context.ColliderBounds = colliders;
                actor.Behaviors.Context.ColliderBodies = colliders.Select(c => _rigidBodyByEntityID[c.EntityID]);

                foreach (var property in actor.Properties.Where(p => !p.Value.IsConstant))
                {
                    actor.Behaviors.Context.SetProperty(property.Key, property.Value);
                }

                actor.Behaviors.Tick();

                if (actor.Behaviors.Context.Translation != Vector3.Zero)
                {
                    HandleActorCollisions(actor, actor.Behaviors.Context.Translation, shape, actor.Behaviors.Context.ColliderBodies);
                    actor.Behaviors.Context.Translation = Vector3.Zero;
                }

                //Rotation = Quaternion.FromEulerAngles(Behaviors.Context.Rotation);
                //Model.Rotation = Behaviors.Context.QRotation;
            }

            if (actor is AnimatedActor animatedActor)
            {
                animatedActor.UpdateAnimation();
            }
        }

        public virtual void HandleActorCollisions(Actor actor, Vector3 translation, IShape shape, IEnumerable<PhysicsBody> colliders)
        {
            if (/*HasCollision && */shape != null && translation != Vector3.Zero)
            {
                //var translatedPosition = actor.Position + translation;

                foreach (var collider in colliders)
                {
                    var colliderPosition = _entityProvider.GetEntity(collider.EntityID).Position;

                    if (Shape3D.Collides(new Vector3(actor.Position.X + translation.X, actor.Position.Y, actor.Position.Z), (Shape3D)shape, colliderPosition, (Shape3D)collider.Shape))
                    {
                        translation.X = 0;
                    }

                    if (Shape3D.Collides(new Vector3(actor.Position.X, actor.Position.Y + translation.Y, actor.Position.Z), (Shape3D)shape, colliderPosition, (Shape3D)collider.Shape))
                    {
                        translation.Y = 0;
                    }

                    if (Shape3D.Collides(new Vector3(actor.Position.X, actor.Position.Y, actor.Position.Z + translation.Z), (Shape3D)shape, colliderPosition, (Shape3D)collider.Shape))
                    {
                        translation.Z = 0;
                    }

                    /*if (collider.AttachedEntity is ICollidable collidable && collidable.HasCollision)
                    {
                        switch (collider.Collider)
                        {
                            case BoundingCircle circle:
                                if (Bounds.CollidesWith(circle))
                                {
                                    // Correct the X translation
                                    Bounds.Center = new Vector3(Position.X + translation.X, Position.Y, Position.Z);
                                    if (Bounds.CollidesWith(circle))
                                    {
                                        translation.X = 0;
                                    }

                                    // Correct the Y translation
                                    Bounds.Center = new Vector3(Position.X, Position.Y + translation.Y, Position.Z);
                                    if (Bounds.CollidesWith(circle))
                                    {
                                        translation.Y = 0;
                                    }
                                }
                                break;

                            case BoundingBox box:
                                if (Bounds.CollidesWith(box))
                                {
                                    // Correct the X translation
                                    Bounds.Center = new Vector3(Position.X + translation.X, Position.Y, Position.Z);
                                    if (Bounds.CollidesWith(box))
                                    {
                                        translation.X = 0;
                                    }

                                    // Correct the Y translation
                                    Bounds.Center = new Vector3(Position.X, Position.Y + translation.Y, Position.Z);
                                    if (Bounds.CollidesWith(box))
                                    {
                                        translation.Y = 0;
                                    }
                                }
                                break;
                        }
                    }*/
                }
            }

            actor.Position += translation;
        }
    }
}
