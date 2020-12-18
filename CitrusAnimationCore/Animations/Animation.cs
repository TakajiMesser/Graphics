using System.Collections.Generic;

namespace CitrusAnimationCore.Animations
{
    public class Animation : IAnimation
    {
        public Animation(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }

        public string Name { get; }
        public float Duration { get; }

        public List<IKeyFrame> KeyFrames { get; set; } = new List<IKeyFrame>();
    }
}
