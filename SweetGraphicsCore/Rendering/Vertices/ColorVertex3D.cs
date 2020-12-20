using SpiceEngineCore.Rendering.Matrices;
using System.Runtime.InteropServices;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
