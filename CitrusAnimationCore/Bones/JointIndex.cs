namespace CitrusAnimationCore.Bones
{
    public struct JointIndex
    {
        public JointIndex(int meshIndex, int boneIndex)
        {
            MeshIndex = meshIndex;
            BoneIndex = boneIndex;
        }

        public int MeshIndex { get; }
        public int BoneIndex { get; }
    }
}
