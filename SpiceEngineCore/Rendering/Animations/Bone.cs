using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Animations
{
    public class Bone
    {
        public string Name { get; set; }
        public List<Bone> Children { get; set; } = new List<Bone>();
    }
}
