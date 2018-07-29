using OpenTK;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureVertex : IVertex
    {
        public Vector2 Position;
        public Vector2 TextureCoords;

        public TextureVertex(Vector2 position, Vector2 textureCoords)
        {
            Position = position;
            TextureCoords = textureCoords;
        }
    }
}
