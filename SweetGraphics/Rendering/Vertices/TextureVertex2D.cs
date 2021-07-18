using SpiceEngineCore.Geometry;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureVertex2D : IVertex2D
    {
        public TextureVertex2D(Vector2 position, Vector2 textureCoords)
        {
            Position = position;
            TextureCoords = textureCoords;
        }

        public Vector2 Position { get; private set; }
        public Vector2 TextureCoords { get; private set; }
    }
}
