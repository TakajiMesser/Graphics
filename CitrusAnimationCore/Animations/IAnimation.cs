using System.Collections.Generic;

namespace CitrusAnimationCore.Animations
{
    public interface IAnimation
    {
        string Name { get; }
        float Duration { get; }
        List<IKeyFrame> KeyFrames { get; }
    }
}
