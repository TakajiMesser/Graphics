using OpenTK;
using SpiceEngineCore.Components;
using SpiceEngineCore.Physics.Collisions;

namespace SpiceEngineCore.Physics.Shapes
{
    public interface IShape : IComponent
    {
        IPartition ToPartition(Vector3 position);
        //IShape Duplicate();
        float CalculateInertia(float mass);
    }
}
