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
    public struct TextureVertex2D : IVertex2D
    {
        public Vector2 Position { get; private set; }
        public Vector2 TextureCoords { get; private set; }

        public TextureVertex2D(Vector2 position, Vector2 textureCoords)
        {
            Position = position;
            TextureCoords = textureCoords;
        }
    }
}
