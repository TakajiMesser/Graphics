using SpiceEngineCore.Rendering.Matrices;
using System.Runtime.InteropServices;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Vertices
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
