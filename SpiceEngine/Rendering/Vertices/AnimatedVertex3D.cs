using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AnimatedVertex3D : IVertex3D, ITextureVertex, IColorVertex, IAnimatedVertex
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector3 Tangent { get; private set; }
        public Vector2 TextureCoords { get; private set; }
        public Color4 Color { get; private set; }
        public Vector4 BoneIDs { get; private set; }
        public Vector4 BoneWeights { get; private set; }

        public AnimatedVertex3D(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords, Vector4 boneIDs, Vector4 boneWeights)
        {
            Position = position;
            Color = new Color4();
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
            BoneIDs = boneIDs;
            BoneWeights = boneWeights;
        }

        public IVertex3D Transformed(Matrix4 modelMatrix) => new AnimatedVertex3D()
        {
            Position = (modelMatrix * new Vector4(Position, 1.0f)).Xyz,
            Color = Color,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights
        };

        public IColorVertex Colored(Color4 color)
        {
            return new AnimatedVertex3D()
            {
                Position = Position,
                Color = color,
                Normal = Normal,
                Tangent = Tangent,
                TextureCoords = TextureCoords,
                BoneIDs = BoneIDs,
                BoneWeights = BoneWeights
            };
        }
    }
}
