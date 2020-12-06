using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Vectors;

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
