using System;

namespace CitrusAnimationCore.Animations
{
    public class AnimationEventArgs : EventArgs
    {
        public IAnimation Animation { get; private set; }

        public AnimationEventArgs(IAnimation animation) => Animation = animation;
    }
}
