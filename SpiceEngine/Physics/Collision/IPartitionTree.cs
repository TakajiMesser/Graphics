using System.Collections.Generic;

namespace SpiceEngine.Physics.Collision
{
    public interface IPartitionTree
    {
        int Level { get; set; }
        List<Bounds> Colliders { get; }

        void Insert(Bounds collider);
        void InsertRange(IEnumerable<Bounds> colliders);
        IEnumerable<Bounds> Retrieve(Bounds collider);
        void Split();
        void Clear();
    }
}
