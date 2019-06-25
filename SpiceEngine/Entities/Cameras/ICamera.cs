using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Inputs;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using System.Collections.Generic;

namespace SpiceEngine.Entities.Cameras
{
    public interface ICamera : IEntity
    {
        int ID { get; set; }
        string Name { get; }
        Vector3 Position { get; set; }
        IEntity AttachedEntity { get; }
        Vector3 AttachedTranslation { get; }

        Matrix4 ViewMatrix { get; }
        Matrix4 ProjectionMatrix { get; }
        Matrix4 ViewProjectionMatrix { get; }

        void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation);
        void DetachFromEntity();

        void OnUpdateFrame();
        void OnHandleInput(InputManager inputManager);

        void SetUniforms(ShaderProgram program);
        void SetUniforms(ShaderProgram program, PointLight light);
        void SetUniforms(ShaderProgram program, SpotLight light);
    }
}
