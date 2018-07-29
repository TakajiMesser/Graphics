using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex : IVertex
    {
        public Vector3 Position;
        public Vector4 Color;

        public ColorVertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
