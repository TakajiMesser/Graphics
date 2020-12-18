using SpiceEngineCore.Geometry.Matrices;
using System.Collections.Generic;

namespace CitrusAnimationCore.Animations
{
    public class MeshTransforms
    {
        public MeshTransforms(int meshIndex) => MeshIndex = meshIndex;

        public int MeshIndex { get; private set; }
        public Dictionary<int, Matrix4> TransformsByBoneIndex { get; private set; } = new Dictionary<int, Matrix4>();
    }
}
