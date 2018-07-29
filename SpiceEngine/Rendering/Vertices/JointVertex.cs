using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JointVertex : IVertex
    {
        public Vector3 Position;
        public Color4 Color;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector2 TextureCoords;
        public Vector4 BoneIDs;
        public Vector4 BoneWeights;

        public JointVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords)
        {
            Position = position;
            Color = new Color4();
            Normal = normal;
            Tangent = tangent;
            TextureCoords = textureCoords;
            BoneIDs = new Vector4();
            BoneWeights = new Vector4();
        }

        public JointVertex Colored(Color4 color)
        {
            return new JointVertex()
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
