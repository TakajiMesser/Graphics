using CitrusAnimationCore.Animations;
using SpiceEngineCore.Components;
using System;

namespace CitrusAnimationCore
{
    public interface IAnimator : IComponent
    {
        IAnimation DefaultAnimation { get; set; }
        IAnimation CurrentAnimation { get; set; }

        event EventHandler<KeyFrameEventArgs> Animate;
        event EventHandler<AnimationEventArgs> AnimationEnd;

        void Tick(float nTicks);
        void Seek(float tick);
    }
}
