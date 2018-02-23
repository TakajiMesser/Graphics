using Graphics.Materials;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Vertices
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

        public void Load(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Color = new Color4();
            TextureCoords = textureCoords;
        }

        public void Load(Vector3 position, Vector3 normal, Vector3 tangent, Color4 color, Vector2 textureCoords)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Color = color;
            TextureCoords = textureCoords;
        }
    }
}
