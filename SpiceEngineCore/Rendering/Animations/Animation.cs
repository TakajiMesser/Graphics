using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Animations
{
    public class Animation
    {
        public string Name { get; private set; }
        public float Duration { get; set; }
        public List<KeyFrame> KeyFrames { get; private set; } = new List<KeyFrame>();

        public Animation(string name) => Name = name;
    }
}
