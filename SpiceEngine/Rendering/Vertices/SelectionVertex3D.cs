using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SelectionVertex3D : IVertex3D, IColorVertex
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector3 Tangent { get; private set; }
        public Vector2 TextureCoords { get; private set; }
        public Color4 Color { get; private set; }
        public Vector4 BoneIDs { get; private set; }
        public Vector4 BoneWeights { get; private set; }
        public Color4 ID { get; private set; }

        public SelectionVertex3D(Vertex3D vertex, Color4 id)
        {
            Position = vertex.Position;
            Normal = vertex.Normal;
            Tangent = vertex.Tangent;
            TextureCoords = vertex.TextureCoords;
            Color = vertex.Color;
            BoneIDs = Vector4.Zero;
            BoneWeights = Vector4.Zero;
            ID = id;
        }

        public SelectionVertex3D(JointVertex3D vertex, Color4 id)
        {
            Position = vertex.Position;
            Normal = vertex.Normal;
            Tangent = vertex.Tangent;
            TextureCoords = vertex.TextureCoords;
            Color = vertex.Color;
            BoneIDs = vertex.BoneIDs;
            BoneWeights = vertex.BoneWeights;
            ID = id;
        }

        public IVertex3D Transformed(Matrix4 modelMatrix) => new SelectionVertex3D()
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

        public IColorVertex Colored(Color4 color) => new SelectionVertex3D()
        {
            Position = Position,
            Color = color,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights,
            ID = ID
        };
    }
}
