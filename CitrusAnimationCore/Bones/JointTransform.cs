using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace CitrusAnimationCore.Bones
{
    public class JointTransform
    {
        private List<JointIndex> _jointIndices = new List<JointIndex>();

        public string Name { get; set; }

        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Matrix4 Transform => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);

        public void AddJointIndex(int meshIndex, int boneIndex) => _jointIndices.Add(new JointIndex(meshIndex, boneIndex));
        
        public JointIndex? GetJointIndex(int meshIndex)
        {
            foreach (var jointIndex in _jointIndices)
            {
                if (jointIndex.MeshIndex == meshIndex)
                {
                    return jointIndex;
                }
            }

            return null;
        }
    }
}
