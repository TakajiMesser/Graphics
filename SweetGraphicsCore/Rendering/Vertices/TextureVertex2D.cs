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
