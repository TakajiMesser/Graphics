using SpiceEngineCore.Rendering.Animations;
using System.Collections.Generic;

namespace SpiceEngineCore.Components.Animations
{
    public class Animation
    {
        public Animation(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }

        public string Name { get; }
        public float Duration { get; }

        public List<KeyFrame> KeyFrames { get; set; } = new List<KeyFrame>();
    }
}
