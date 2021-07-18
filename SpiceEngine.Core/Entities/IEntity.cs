using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Matrices;
using System;

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
