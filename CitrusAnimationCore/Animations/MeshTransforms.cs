using OpenTK;
using SpiceEngineCore.Utilities;

namespace CitrusAnimationCore.Animations
{
    public class MeshTransforms
    {
        public const int MAX_JOINTS = 100;

        public MeshTransforms(int meshIndex) => MeshIndex = meshIndex;

        public int MeshIndex { get; }
        public Matrix4[] Transforms { get; } = ArrayExtensions.Initialize(MAX_JOINTS, Matrix4.Zero);

        public void AddTransform(int boneIndex, Matrix4 transform) => Transforms[boneIndex] = transform;
    }
}
