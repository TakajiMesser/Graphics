using System.Collections.Generic;

namespace SpiceEngineCore.Components.Animations
{
    public interface IAnimation
    {
        string Name { get; }
        float Duration { get; }
        List<IKeyFrame> KeyFrames { get; }
    }
}
