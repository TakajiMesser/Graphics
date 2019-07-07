using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex3D : IVertex3D, IColorVertex
    {
        public Vector3 Position { get; private set; }
        public Color4 Color { get; private set; }

        public ColorVertex3D(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        public IVertex3D Transformed(Matrix4 modelMatrix) => new ColorVertex3D()
        {
            Position = (modelMatrix * new Vector4(Position, 1.0f)).Xyz,
            Color = Color
        };

        public IColorVertex Colored(Color4 color) => new ColorVertex3D()
        {
            Position = Position,
            Color = color
        };
    }
}
