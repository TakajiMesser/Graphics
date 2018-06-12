using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace TakoEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex : IVertex
    {
        public Vector3 Position;
        public Color4 Color;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector2 TextureCoords;

        public Vertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords)
        {
            Position = position;
            Color = new Color4();
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
        }

        public Vertex Colored(Color4 color)
        {
            return new Vertex()
            {
                Position = Position,
                Color = color,
                Normal = Normal,
                Tangent = Tangent,
                TextureCoords = TextureCoords,
            };
        }
    }
}
