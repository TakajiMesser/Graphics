using SavoryPhysicsCore.Shapes;
using SpiceEngineCore.Components;
using SpiceEngineCore.Geometry.Quaternions;
using SpiceEngineCore.Geometry.Vectors;

namespace SavoryPhysicsCore
{
    public enum BodyStates
    {
        Awake,
        Asleep
    }

    public interface IBody : IComponent
    {
        IShape Shape { get; }
        BodyStates State { get; set; }

        bool IsMovable { get; }
        bool IsPhysical { get; set; }

        Vector3 UpdatePosition(Vector3 position, int nTicks);
        Quaternion UpdateRotation(Quaternion rotation, int nTicks);

        void ApplyImpulse(Vector3 impulse);
        void ApplyVelocity(Vector3 velocity);
        void ApplyForce(Vector3 force);
        void ApplyPreciseForce(Vector3 force, Vector3 point);
    }
}
