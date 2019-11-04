using SpiceEngineCore.Rendering.Animations;
using System;

namespace SpiceEngineCore.Components.Animations
{
    public interface IAnimator : IComponent
    {
        Animation DefaultAnimation { get; set; }
        Animation CurrentAnimation { get; set; }

        void Tick(float nTicks);
        void Seek(float tick);

        event EventHandler<KeyFrameEventArgs> Animate;
        event EventHandler<AnimationEventArgs> AnimationEnd;
    }
}
