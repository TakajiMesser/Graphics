using OpenTK;
using System;

namespace SpiceEngine.Entities
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
