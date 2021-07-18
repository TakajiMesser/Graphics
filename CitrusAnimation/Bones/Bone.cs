using System.Collections.Generic;

namespace CitrusAnimationCore.Bones
{
    public class Bone
    {
        public string Name { get; set; }
        public List<Bone> Children { get; set; } = new List<Bone>();
    }
}
