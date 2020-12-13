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
    public class TextureTransformEventArgs : EventArgs
    {
        public int ID { get; }
        public Vector2 Translation { get; }
        public float Rotation { get; }
        public Vector2 Scale { get; }

        public TextureTransformEventArgs(int id, Vector2 translation, float rotation, Vector2 scale)
        {
            ID = id;
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
