using SpiceEngineCore.Components.Animations;
using System;

namespace SpiceEngineCore.Components.Animations
{
    public class AnimationEventArgs : EventArgs
    {
        public IAnimation Animation { get; private set; }

        public AnimationEventArgs(IAnimation animation) => Animation = animation;
    }
}
