using OpenTK;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public class QuadTree : IPartitionTree
    {
        public const int NUMBER_OF_NODES = 4;
        public const int MAX_COLLIDERS = 10;
        public const int MAX_LEVELS = 5;

        public int Level { get; set; }
        public Quad Quad { get; set; }
        public List<Bounds> Colliders { get; set; } = new List<Bounds>();
        public QuadTree[] Nodes { get; set; } = null;

        public QuadTree(int level, Quad quad)
        {
            Level = level;
            Quad = quad;
        }

        public void InsertRange(IEnumerable<Bounds> colliders)
        {
            foreach (var collider in colliders)
            {
                Insert(collider);
            }
        }

        public void Insert(Bounds collider)
        {
            // When we insert a new collider, we need to check a few things
            // First, is this quad already split up? If not, we can just insert into the parent quad
            if (TryGetNodeIndex(collider, out int index))
            {
                Nodes[index].Insert(collider);
            }
            else
            {
                Colliders.Add(collider);

                // This collider belongs in the parent quad, but we need to ensure that we aren't going to overflow
                if (Colliders.Count > MAX_COLLIDERS && Level < MAX_LEVELS && Nodes == null)
                {
                    Split();

                    // Go through each parent collider, and determine if they need to be reassigned to the newly split nodes
                    for (var i = Colliders.Count - 1; i >= 0; i--)
                    {
                        if (TryGetNodeIndex(Colliders[i], out int indexB))
                        {
                            Nodes[indexB].Insert(Colliders[i]);
                            Colliders.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public IEnumerable<Bounds> Retrieve(Bounds collider)
        {
            var colliders = new List<Bounds>(Colliders);

            if (TryGetNodeIndex(collider, out int index))
            {
                colliders.AddRange(Nodes[index].Retrieve(collider));
            }

            return colliders;
        }

        public void Split()
        {
            var halfWidth = (Quad.Max.X - Quad.Min.X) / 2.0f;
            var halfHeight = (Quad.Max.Y - Quad.Min.Y) / 2.0f;

            Nodes = new QuadTree[NUMBER_OF_NODES];

            Nodes[0] = new QuadTree(Level + 1, new Quad(new Vector2(Quad.Min.X + halfWidth, Quad.Min.Y + halfHeight), Quad.Max));
            Nodes[1] = new QuadTree(Level + 1, new Quad(new Vector2(Quad.Min.X, Quad.Min.Y + halfHeight), new Vector2(Quad.Min.X + halfWidth, Quad.Max.Y)));
            Nodes[2] = new QuadTree(Level + 1, new Quad(Quad.Min, new Vector2(Quad.Min.X + halfWidth, Quad.Min.Y + halfHeight)));
            Nodes[3] = new QuadTree(Level + 1, new Quad(new Vector2(Quad.Min.X + halfWidth, Quad.Min.Y), new Vector2(Quad.Max.X, Quad.Min.Y + halfHeight)));
        }

        public void Clear()
        {
            Colliders.Clear();

            if (Nodes != null)
            {
                for (var i = 0; i < NUMBER_OF_NODES; i++)
                {
                    Nodes[i].Clear();
                }

                Nodes = null;
            }
        }

        public bool TryGetNodeIndex(Bounds collider, out int index)
        {
            if (Nodes != null)
            {
                // Figure out which node this quad can fit into, if possible
                // If it overlaps, then it is instead a part of the parent node
                for (var i = 0; i < NUMBER_OF_NODES; i++)
                {
                    if (Nodes[i].Quad.CanContain(collider))
                    {
                        index = i;
                        return true;
                    }
                }
            }

            index = -1;
            return false;
        }
    }
}
