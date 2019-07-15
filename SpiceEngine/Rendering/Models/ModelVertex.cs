using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public class ModelVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector2 UV { get; set; }
        public Color4 Color { get; set; }
        public Vector4? BoneIDs { get; set; }
        public Vector4? BoneWeights { get; set; }

        public bool IsAnimated => BoneIDs.HasValue && BoneWeights.HasValue;

        public ModelVertex Duplicated() => new ModelVertex()
        {
            Position = Position,
            Normal = Normal,
            Tangent = Tangent,
            UV = UV,
            Color = Color,
            BoneIDs = BoneIDs,
            BoneWeights = BoneWeights
        };

        public Vertex3D ToVertex3D() => new Vertex3D(Position, Normal, Tangent, UV, Color);

        public AnimatedVertex3D ToJointVertex3D() => (AnimatedVertex3D)new AnimatedVertex3D(Position, Normal, Tangent, UV,
            BoneIDs ?? Vector4.Zero, BoneWeights ?? Vector4.Zero)
            .Colored(Color);
    }
}
