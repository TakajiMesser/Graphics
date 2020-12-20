using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities.Cameras
{
    public interface ICamera : IEntity
    {
        string Name { get; }
        bool IsActive { get; set; }

        IEntity AttachedEntity { get; }
        Vector3 AttachedTranslation { get; }

        Matrix4 CurrentProjectionMatrix { get; }
        Matrix4 PreviousProjectionMatrix { get; }

        void UpdateAspectRatio(float value);

        void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation);
        void DetachFromEntity();
    }
}
