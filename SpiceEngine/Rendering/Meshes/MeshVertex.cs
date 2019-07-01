using OpenTK;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshVertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector2 UV { get; set; }
        public Color4 Color { get; set; }
        public Vector4? BoneIDs { get; set; }
        public Vector4? BoneWeights { get; set; }

        public bool IsAnimated => BoneIDs.HasValue && BoneWeights.HasValue;

        public Vertex3D ToVertex3D() => new Vertex3D(Position, Normal, Tangent, UV, Color);

        public JointVertex3D ToJointVertex3D() => new JointVertex3D(Position, Normal, Tangent, UV,
            BoneIDs.HasValue ? BoneIDs.Value : Vector4.Zero, BoneWeights.HasValue ? BoneWeights.Value : Vector4.Zero)
            .Colored(Color);
    }
}
