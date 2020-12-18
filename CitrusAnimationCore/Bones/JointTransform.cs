using OpenTK;
using System.Collections.Generic;

namespace CitrusAnimationCore.Bones
{
    public class JointTransform
    {
        public string Name { get; set; }
        public List<JointIndex> JointIndices { get; private set; } = new List<JointIndex>();

        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Matrix4 Transform => Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);

        public JointTransform() { }
    }
}
