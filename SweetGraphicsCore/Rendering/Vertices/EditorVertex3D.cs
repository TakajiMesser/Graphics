using SpiceEngineCore.Rendering.Matrices;
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
    public struct EditorVertex3D : IVertex3D, ITextureVertex, IColorVertex, ISelectionVertex
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public Vector3 Tangent { get; private set; }
        public Vector2 TextureCoords { get; private set; }
        public Color4 Color { get; private set; }
        public Vector4 BoneIDs { get; private set; }
        public Vector4 BoneWeights { get; private set; }
        public Color4 IDColor { get; private set; }

        public EditorVertex3D(IVertex3D vertex, Color4 idColor)
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

            IDColor = idColor;
        }

        public IVertex3D Transformed(Transform transform)
        {
            var modelMatrix = transform.ToMatrix();
            var rotationMatrix = Matrix4.CreateFromQuaternion(transform.Rotation);

            return new EditorVertex3D()
            {
                Position = (new Vector4(Position, 1.0f) * modelMatrix).Xyz,
                Normal = (new Vector4(Normal, 1.0f) * rotationMatrix).Xyz,
                Tangent = (new Vector4(Tangent, 1.0f) * rotationMatrix).Xyz,
                TextureCoords = TextureCoords,
                Color = Color,
                BoneIDs = BoneIDs,
                BoneWeights = BoneWeights,
                IDColor = IDColor
            };
        }

        public ITextureVertex TextureTransformed(Vector3 center, Vector2 translation, float rotation, Vector2 scale)
        {
            var bitangent = Vector3.Cross(Normal, Tangent);

            // Rotate in relation to center axis
            // TODO - Determine if this Normal needs to be the Normal of the face, rather than of the vertex
            var rotationQuaternion = new Quaternion(center + Normal, rotation);

            // Handle translation (X bitangent, Y tangent)
            var textureCoords = new Vector2(TextureCoords.X + translation.X, TextureCoords.Y + translation.Y);

            return new EditorVertex3D()
            {
                // For translation, we need to translate the TextureCoords in the Tangent-Vector direction
                Position = Position,
                Normal = Normal,
                Tangent = Tangent, // Tangent will need to get rotated potentially
                TextureCoords = textureCoords, // This will definitely need to get updated...
                Color = Color,
                BoneIDs = BoneIDs,
                BoneWeights = BoneWeights,
                IDColor = IDColor
            };
        }

        public ISelectionVertex Selected() => new EditorVertex3D()
        {
            Position = Position,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
            Color = Color,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights,
            IDColor = new Color4(IDColor.R, IDColor.G, IDColor.B, 0.5f)
        };

        public ISelectionVertex Deselected() => new EditorVertex3D()
        {
            Position = Position,
            Normal = Normal,
            Tangent = Tangent,
            TextureCoords = TextureCoords,
            Color = Color,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights,
            IDColor = new Color4(IDColor.R, IDColor.G, IDColor.B, 1.0f)
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
            IDColor = IDColor
        };
    }
}
