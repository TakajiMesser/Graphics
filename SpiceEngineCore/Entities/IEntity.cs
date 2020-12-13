using SpiceEngineCore.Rendering.Matrices;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
