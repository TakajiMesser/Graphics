using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Matrices;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex3D : IVertex3D, ITextureVertex, IColorVertex
    {
        public Vertex3D(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords) : this(position, normal, tangent, textureCoords, new Color4()) { }
        public Vertex3D(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords, Color4 color)
        {
            Position = position;
            Color = color;
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
        }

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector2 TextureCoords { get; set; }
        public Color4 Color { get; set; }

        public IVertex3D Transformed(Transform transform)
        {
            var matrix = transform.ToMatrix();

            return new Vertex3D()
            {
                Position = (new Vector4(Position, 1.0f) * matrix).Xyz,
                Normal = Normal,
                Tangent = Tangent,
                TextureCoords = TextureCoords,
                Color = Color
            };
        }

        public ITextureVertex TextureTransformed(Vector3 center, Vector2 translation, float rotation, Vector2 scale) => null;

        public IColorVertex Colored(Color4 color) => new Vertex3D()
        {
            Position = Position,
            Color = color,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
        };
    }
}
