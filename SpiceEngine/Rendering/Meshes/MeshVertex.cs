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
        public Vector4? BoneIDs { get; set; }
        public Vector4? BoneWeights { get; set; }
    }
}
