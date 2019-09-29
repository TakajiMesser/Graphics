using OpenTK;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Animations
{
    public class MeshTransforms
    {
        public int MeshIndex { get; private set; }
        public Dictionary<int, Matrix4> TransformsByBoneIndex { get; private set; } = new Dictionary<int, Matrix4>();

        public MeshTransforms(int meshIndex) => MeshIndex = meshIndex;
    }
}
