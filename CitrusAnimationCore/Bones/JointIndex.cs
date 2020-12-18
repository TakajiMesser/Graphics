namespace CitrusAnimationCore.Bones
{
    // TODO - Can this be turned into a struct?
    public class JointIndex
    {
        public int MeshIndex { get; private set; }
        public int BoneIndex { get; private set; }

        public JointIndex(int meshIndex, int boneIndex)
        {
            MeshIndex = meshIndex;
            BoneIndex = boneIndex;
        }
    }
}
