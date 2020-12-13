using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Entities.Cameras
{
    public interface ICamera : IEntity
    {
        string Name { get; }
        bool IsActive { get; set; }

        IEntity AttachedEntity { get; }
        Vector3 AttachedTranslation { get; }

        Matrix4 ViewMatrix { get; }
        Matrix4 PreviousViewMatrix { get; }
        Matrix4 ProjectionMatrix { get; }
        Matrix4 PreviousProjectionMatrix { get; }

        void UpdateAspectRatio(float value);

        void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation);
        void DetachFromEntity();
    }
}
