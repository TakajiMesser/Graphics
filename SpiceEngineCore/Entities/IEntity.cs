using OpenTK;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using System;

namespace SpiceEngineCore.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }

        event EventHandler<EntityTransformEventArgs> Transformed;

        void Transform(Transform transform);
        void SetUniforms(ShaderProgram shader);
        bool CompareUniforms(IEntity entity);
    }
}
