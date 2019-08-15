using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using System;

namespace SpiceEngine.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }

        void Transform(Transform transform);
        void SetUniforms(ShaderProgram shader);
        bool CompareUniforms(IEntity entity);

        event EventHandler<EntityTransformEventArgs> Transformed;
    }
}
