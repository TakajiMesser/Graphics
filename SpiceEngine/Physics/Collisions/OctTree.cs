using OpenTK;
using System.Collections.Generic;

namespace SpiceEngine.Physics.Collisions
{
    public class OctTree : IPartitionTree
    {
        public const int NUMBER_OF_NODES = 8;
        public const int MAX_COLLIDERS = 10;
        public const int MAX_LEVELS = 5;

        public int Level { get; set; }
        public Oct Oct { get; set; }
        public List<Bounds> Colliders { get; set; } = new List<Bounds>();
        public OctTree[] Nodes { get; set; } = null;

        public OctTree(int level, Oct oct)
        {
            Level = level;
            Oct = oct;
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
            var halfWidth = (Oct.Max.X - Oct.Min.X) / 2.0f;
            var halfHeight = (Oct.Max.Y - Oct.Min.Y) / 2.0f;
            var halfDepth = (Oct.Max.Z - Oct.Min.Z) / 2.0f;

            Nodes = new OctTree[NUMBER_OF_NODES];

            Nodes[0] = new OctTree(Level + 1, new Oct(new Vector3(Oct.Min.X + halfWidth, Oct.Min.Y + halfHeight, Oct.Min.Z), Oct.Max));
            Nodes[1] = new OctTree(Level + 1, new Oct(new Vector3(Oct.Min.X, Oct.Min.Y + halfHeight, Oct.Min.Z), new Vector3(Oct.Min.X + halfWidth, Oct.Max.Y, Oct.Max.Z)));
            Nodes[2] = new OctTree(Level + 1, new Oct(Oct.Min, new Vector3(Oct.Min.X + halfWidth, Oct.Min.Y + halfHeight, Oct.Max.Z)));
            Nodes[3] = new OctTree(Level + 1, new Oct(new Vector3(Oct.Min.X + halfWidth, Oct.Min.Y, Oct.Min.Z), new Vector3(Oct.Max.X, Oct.Min.Y + halfHeight, Oct.Max.Z)));

            // TODO - Correct these octal boundaries
            Nodes[4] = new OctTree(Level + 1, new Oct(new Vector3(Oct.Min.X + halfWidth, Oct.Min.Y + halfHeight, Oct.Min.Z + halfDepth), Oct.Max));
            Nodes[5] = new OctTree(Level + 1, new Oct(new Vector3(Oct.Min.X, Oct.Min.Y + halfHeight, Oct.Min.Z + halfDepth), new Vector3(Oct.Min.X + halfWidth, Oct.Max.Y, Oct.Max.Z)));
            Nodes[6] = new OctTree(Level + 1, new Oct(Oct.Min, new Vector3(Oct.Min.X + halfWidth, Oct.Min.Y + halfHeight, Oct.Min.Z + halfDepth)));
            Nodes[7] = new OctTree(Level + 1, new Oct(new Vector3(Oct.Min.X + halfWidth, Oct.Min.Y, Oct.Min.Z + halfDepth), new Vector3(Oct.Max.X, Oct.Min.Y + halfHeight, Oct.Max.Z)));
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
                // Figure out which node this oct can fit into, if possible
                // If it overlaps, then it is instead a part of the parent node
                for (var i = 0; i < NUMBER_OF_NODES; i++)
                {
                    if (Nodes[i].Oct.CanContain(collider))
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
