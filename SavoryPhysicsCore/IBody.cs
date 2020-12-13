using SavoryPhysicsCore.Shapes;
using SpiceEngineCore.Components;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
