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
    public class EntityTransformEventArgs : EventArgs
    {
        public int ID { get; }
        public Vector3 Position { get; }
        public Transform Transform { get; }

        public EntityTransformEventArgs(int id, Vector3 position, Transform transform)
        {
            ID = id;
            Position = position;
            Transform = transform;
        }
    }
}
