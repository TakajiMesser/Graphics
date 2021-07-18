using SpiceEngineCore.Geometry;
using System;

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

        event EventHandler BecameActive;

        void UpdateAspectRatio(float value);

        void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation);
        void DetachFromEntity();
    }
}
