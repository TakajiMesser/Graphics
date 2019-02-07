using OpenTK;
using SpiceEngine.Physics.Collision;

namespace SpiceEngine.Physics.Shapes
{
    public interface IShape
    {
        float Mass { get; set; }
        float MomentOfInertia { get; }

        ICollider ToCollider(Vector3 position);
        IShape Duplicate();
    }
}
