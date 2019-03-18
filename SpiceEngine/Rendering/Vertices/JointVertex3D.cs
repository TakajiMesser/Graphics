using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JointVertex3D : IVertex3D, IColorVertex
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector3 Tangent { get; private set; }
        public Vector2 TextureCoords { get; private set; }
        public Color4 Color { get; private set; }
        public Vector4 BoneIDs { get; private set; }
        public Vector4 BoneWeights { get; private set; }

        public JointVertex3D(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords, Vector4 boneIDs, Vector4 boneWeights)
        {
            Position = position;
            Color = new Color4();
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
            BoneIDs = boneIDs;
            BoneWeights = boneWeights;
        }

        public IColorVertex Colored(Color4 color)
        {
            return new JointVertex3D()
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
