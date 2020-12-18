using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering.Matrices;
using System;

namespace SpiceEngineCore.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }

        Matrix4 ModelMatrix { get; }
        Matrix4 PreviousModelMatrix { get; }
        ModelMatrix WorldMatrix { get; }

        event EventHandler<EntityTransformEventArgs> Transformed;

        void Transform(Transform transform);
        //void SetUniforms(ShaderProgram shader);
        //bool CompareUniforms(IEntity entity);
    }
}
