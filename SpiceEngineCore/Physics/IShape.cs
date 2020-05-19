using OpenTK;
using SpiceEngineCore.Components;

namespace SpiceEngineCore.Physics
{
    public interface IShape : IComponent
    {
        IPartition ToPartition(Vector3 position);
        //IShape Duplicate();
        float CalculateInertia(float mass);
    }
}
