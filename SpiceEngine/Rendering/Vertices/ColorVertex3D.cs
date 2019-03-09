using OpenTK;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex3D : IVertex3D, IColorVertex
    {
        public Vector3 Position { get; private set; }
        public Vector4 Color { get; private set; }

        public ColorVertex3D(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }
}
