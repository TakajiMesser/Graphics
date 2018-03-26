using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace TakoEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JointVertex// : IVertex
    {
        public Vector3 Position;// { get; set; }
        public Vector3 Normal;// { get; set; }
        public Vector3 Tangent;// { get; set; }
        public Color4 Color;// { get; set; }
        public Vector2 TextureCoords;// { get; set; }
        public Vector4 BoneIDs;// { get; set; }
        public Vector4 BoneWeights;// { get; set; }

        public JointVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Color = new Color4();
            TextureCoords = textureCoords;
            BoneIDs = new Vector4();
            BoneWeights = new Vector4();
        }

        public JointVertex Colored(Color4 color)
        {
            return new JointVertex()
            {
                Position = Position,
                Normal = Normal,
                Tangent = Tangent,
                Color = color,
                TextureCoords = TextureCoords,
                BoneIDs = BoneIDs,
                BoneWeights = BoneWeights
            };
        }
    }
}
