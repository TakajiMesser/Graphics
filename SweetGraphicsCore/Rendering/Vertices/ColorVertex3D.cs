using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Matrices;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex3D : IVertex3D, IColorVertex
    {
        public ColorVertex3D(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position { get; private set; }
        public Color4 Color { get; private set; }

        public IVertex3D Transformed(Transform transform) => new ColorVertex3D()
        {
            Position = (transform.ToMatrix() * new Vector4(Position, 1.0f)).Xyz,
            Color = Color
        };

        public IColorVertex Colored(Color4 color) => new ColorVertex3D()
        {
            Position = Position,
            Color = color
        };
    }
}
