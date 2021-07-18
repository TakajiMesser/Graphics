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
    public class TextureTransformEventArgs : EventArgs
    {
        public TextureTransformEventArgs(int id, Vector2 translation, float rotation, Vector2 scale)
        {
            ID = id;
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        public int ID { get; }
        public Vector2 Translation { get; }
        public float Rotation { get; }
        public Vector2 Scale { get; }
    }
}
