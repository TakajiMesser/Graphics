using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Physics.Collision
{
    public class QuadTree
    {
        public const int NUMBER_OF_NODES = 4;
        public const int MAX_COLLIDERS = 10;
        public const int MAX_LEVELS = 5;

        public int Level { get; set; }
        public Quad Quad { get; set; }
        public List<Collider> Colliders { get; set; } = new List<Collider>();
        public QuadTree[] Nodes { get; set; } = null;

        public QuadTree(int level, Quad quad)
        {
            Level = level;
            Quad = quad;
        }

        public void Insert(Collider collider)
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

        public IEnumerable<Collider> Retrieve(Collider collider)
        {
            var colliders = new List<Collider>(Colliders);

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

            Nodes[0] = new QuadTree(Level + 1, new Quad(new Vector3(Quad.Min.X + halfWidth, Quad.Min.Y + halfHeight, Quad.Min.Z), Quad.Max));
            Nodes[1] = new QuadTree(Level + 1, new Quad(new Vector3(Quad.Min.X, Quad.Min.Y + halfHeight, Quad.Min.Z), new Vector3(Quad.Min.X + halfWidth, Quad.Max.Y, Quad.Max.Z)));
            Nodes[2] = new QuadTree(Level + 1, new Quad(Quad.Min, new Vector3(Quad.Min.X + halfWidth, Quad.Min.Y + halfHeight, Quad.Max.Z)));
            Nodes[3] = new QuadTree(Level + 1, new Quad(new Vector3(Quad.Min.X + halfWidth, Quad.Min.Y, Quad.Min.Z), new Vector3(Quad.Max.X, Quad.Min.Y + halfHeight, Quad.Max.Z)));
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

        public bool TryGetNodeIndex(Collider collider, out int index)
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
