using OpenTK;

namespace SpiceEngine.Rendering.Meshes
{
    public struct UVMap
    {
        public Vector2 Translation { get; private set; }
        public Vector2 Scale { get; private set; }
        public float Rotation { get; private set; }
        
        public UVMap(Vector2 translation, Vector2 scale, float rotation)
        {
            Translation = translation;
            Scale = scale;
            Rotation = rotation;
        }

        public UVMap Translated(Vector2 translation) => new UVMap(translation, Scale, Rotation);

        public UVMap Scaled(Vector2 scale) => new UVMap(Translation, scale, Rotation);

        public UVMap Rotated(float rotation) => new UVMap(Translation, Scale, rotation);

        public static UVMap Standard => new UVMap(Vector2.Zero, Vector2.One, 0.0f);
    }
}
