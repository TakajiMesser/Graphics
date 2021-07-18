using SpiceEngineCore.Utilities;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
