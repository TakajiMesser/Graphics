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
    public struct Vertex3D : IVertex3D, ITextureVertex, IColorVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector2 TextureCoords { get; set; }
        public Color4 Color { get; set; }

        public Vertex3D(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
            Color = new Color4();
        }

        public Vertex3D(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords, Color4 color)
        {
            Position = position;
            Color = color;
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
        }

        public IVertex3D Transformed(Transform transform)
        {
            var matrix = transform.ToMatrix();

            return new Vertex3D()
            {
                Position = (new Vector4(Position, 1.0f) * matrix).Xyz,
                Normal = Normal,
                Tangent = Tangent,
                TextureCoords = TextureCoords,
                Color = Color
            };
        }

        public ITextureVertex TextureTransformed(Vector3 center, Vector2 translation, float rotation, Vector2 scale) => null;

        public IColorVertex Colored(Color4 color) => new Vertex3D()
        {
            Position = Position,
            Color = color,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
        };
    }
}
