using SpiceEngine.Entities;
using SpiceEngine.Physics.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Physics
{
    public class PhysicsManager
    {
        private ICollisionTree _actorTree;
        private ICollisionTree _brushTree;
        private ICollisionTree _volumeTree;
        private ICollisionTree _lightTree;

        private Dictionary<int, PhysicsBody> _bodiesByEntityID = new Dictionary<int, PhysicsBody>();

        public PhysicsManager(Quad worldBoundaries)
        {
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

        public PhysicsManager(Oct worldBoundaries)
        {
            _actorTree = new OctTree(0, worldBoundaries);
            _brushTree = new OctTree(0, worldBoundaries);
            _volumeTree = new OctTree(0, worldBoundaries);
            _lightTree = new OctTree(0, worldBoundaries);
        }

        public void InsertBrushes(IEnumerable<Bounds> colliders)
        {
            _brushTree.InsertRange(colliders);
        }

        public void InsertVolumes(IEnumerable<Bounds> colliders)
        {
            _volumeTree.InsertRange(colliders);
        }

        public void InsertLights(IEnumerable<Bounds> colliders)
        {
            _lightTree.InsertRange(colliders);
        }

        public void Update(IEnumerable<Actor> actors)
        {
            // Update the gameobject colliders every frame, since they could have moved
            _actorTree.Clear();
            _actorTree.InsertRange(actors.Select(g => g.Bounds).Where(c => c != null));

            // For each object that has a non-zero transform, we need to determine the set of colliders to compare it against for hit detection
            foreach (var actor in actors)
            {
                //actor.ClearLights();
                //actor.AddPointLights(_lightQuads.Retrieve(actor.Bounds)
                //    .Where(c => c.AttachedEntity is PointLight)
                //    .Select(c => (PointLight)c.AttachedEntity));

                var filteredColliders = _brushTree.Retrieve(actor.Bounds)
                    .Concat(_actorTree
                        .Retrieve(actor.Bounds)
                            .Where(c => ((Actor)c.AttachedEntity).Name != actor.Name));

                actor.OnUpdateFrame(filteredColliders);
            }
        }
    }
}
