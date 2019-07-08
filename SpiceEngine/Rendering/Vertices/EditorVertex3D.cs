using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EditorVertex3D : IVertex3D, IColorVertex
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector3 Tangent { get; private set; }
        public Vector2 TextureCoords { get; private set; }
        public Color4 Color { get; private set; }
        public Vector4 BoneIDs { get; private set; }
        public Vector4 BoneWeights { get; private set; }
        public Color4 ID { get; private set; }

        public EditorVertex3D(IVertex3D vertex, Color4 id)
        {
            Position = vertex.Position;

            if (vertex is ITextureVertex textureVertex)
            {
                Normal = textureVertex.Normal;
                Tangent = textureVertex.Tangent;
                TextureCoords = textureVertex.TextureCoords;
            }
            else
            {
                Normal = Vector3.Zero;
                Tangent = Vector3.Zero;
                TextureCoords = Vector2.Zero;
            }

            Color = vertex is IColorVertex colorVertex ? colorVertex.Color : new Color4();

            if (vertex is IAnimatedVertex animatedVertex)
            {
                BoneIDs = animatedVertex.BoneIDs;
                BoneWeights = animatedVertex.BoneWeights;
            }
            else
            {
                BoneIDs = Vector4.Zero;
                BoneWeights = Vector4.Zero;
            }
            
            ID = id;
        }

        public IVertex3D Transformed(Matrix4 modelMatrix) => new EditorVertex3D()
        {
            Position = (new Vector4(Position, 1.0f) * modelMatrix).Xyz,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
            Color = Color,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights,
            ID = ID
        };

        public IColorVertex Colored(Color4 color) => new EditorVertex3D()
        {
            Position = Position,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
            Color = color,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights,
            ID = ID
        };
    }
}
