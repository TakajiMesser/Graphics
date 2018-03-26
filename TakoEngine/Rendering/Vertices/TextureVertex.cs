using OpenTK;
using System.Runtime.InteropServices;

namespace TakoEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TextureVertex// : IVertex
    {
        public Vector2 Position;// { get; set; }
        public Vector2 TextureCoords;// { get; set; }

        public TextureVertex(Vector2 position, Vector2 textureCoords)
        {
            Position = position;
            TextureCoords = textureCoords;
        }
    }
}
