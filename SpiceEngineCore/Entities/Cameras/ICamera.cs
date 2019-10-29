using OpenTK;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngineCore.Entities.Cameras
{
    public interface ICamera : IEntity
    {
        string Name { get; }
        IEntity AttachedEntity { get; }
        Vector3 AttachedTranslation { get; }

        Matrix4 ViewMatrix { get; }
        Matrix4 ProjectionMatrix { get; }
        Matrix4 ViewProjectionMatrix { get; }

        void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation);
        void DetachFromEntity();

        void OnUpdateFrame();
        void OnHandleInput(InputManager inputManager);

        void SetUniforms(ShaderProgram program, ILight light);
    }
}
