using SavoryPhysicsCore.Shapes;
using SpiceEngineCore.Components;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
