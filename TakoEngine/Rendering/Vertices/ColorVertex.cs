using OpenTK;
using System.Runtime.InteropServices;

namespace TakoEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex// : IVertex
    {
        public Vector3 Position;// { get; set; }
        public Vector4 Color;// { get; set; }

        public ColorVertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
