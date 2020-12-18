using OpenTK;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngineCore.Entities.Cameras
{
    public interface ICamera : IEntity
    {
        string Name { get; }
        bool IsActive { get; set; }

        IEntity AttachedEntity { get; }
        Vector3 AttachedTranslation { get; }

        Matrix4 ViewMatrix { get; }
        Matrix4 ProjectionMatrix { get; }
        Matrix4 ViewProjectionMatrix { get; }

        void UpdateAspectRatio(float value);

        void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation);
        void DetachFromEntity();

        void SetUniforms(ShaderProgram program);
        void SetUniforms(ShaderProgram program, ILight light);
    }
}
