using System;

namespace CitrusAnimationCore.Animations
{
    public class AnimationEventArgs : EventArgs
    {
        public AnimationEventArgs(IAnimation animation) => Animation = animation;

        public IAnimation Animation { get; private set; }
    }
}
