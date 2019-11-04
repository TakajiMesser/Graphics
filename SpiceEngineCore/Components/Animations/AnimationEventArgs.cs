using SpiceEngineCore.Rendering.Animations;
using System;

namespace SpiceEngineCore.Components.Animations
{
    public class AnimationEventArgs : EventArgs
    {
        public Animation Animation { get; private set; }

        public AnimationEventArgs(Animation animation) => Animation = animation;
    }
}
