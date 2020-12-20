using SpiceEngineCore.Rendering.Matrices;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }
        Matrix4 CurrentModelMatrix { get; }
        Matrix4 PreviousModelMatrix { get; }

        event EventHandler<EntityTransformEventArgs> Transformed;

        void Transform(Transform transform);
    }
}
