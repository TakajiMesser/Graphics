using SpiceEngineCore.Components.Animations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Components.Animations
{
    public class Animator : IAnimator
    {
        public Animation DefaultAnimation { get; set; }
        public Animation CurrentAnimation { get; set; }
        public float AnimationTick { get; private set; }

        public event EventHandler<KeyFrameEventArgs> Animate;
        public event EventHandler<AnimationEventArgs> AnimationEnd;

        public void Tick(float nTicks)
        {
            if (CurrentAnimation == null) throw new InvalidOperationException("No current animation defined");

            // Find the corresponding keyframe for this tick
            var keyFrame = CurrentAnimation.KeyFrames.FirstOrDefault(k => k.Time == AnimationTick);
            if (keyFrame != null)
            {
                Animate?.Invoke(this, new KeyFrameEventArgs(keyFrame));
            }

            AnimationTick += nTicks;

            // For now, always repeat the animation upon completion
            if (AnimationTick > CurrentAnimation.Duration)
            {
                AnimationTick = 0.0f;

                var animation = CurrentAnimation;
                CurrentAnimation = null;

                AnimationEnd?.Invoke(this, new AnimationEventArgs(animation));
            }
        }

        public void Seek(float tick)
        {
            if (CurrentAnimation == null) throw new InvalidOperationException("No current animation defined");

            // Find the corresponding keyframe for this tick
            /*var keyFrame = CurrentAnimation.KeyFrames.FirstOrDefault(k => k.Time == AnimationTick);
            if (keyFrame != null)
            {
                Animate?.Invoke(this, new KeyFrameEventArgs(keyFrame));
            }*/

            // TODO - Check if this animation should repeat. If not, throw if tick is past duration
            AnimationTick = tick;

            if (AnimationTick > CurrentAnimation.Duration)
            {
                AnimationTick = 0.0f;

                var animation = CurrentAnimation;
                CurrentAnimation = null;

                AnimationEnd?.Invoke(this, new AnimationEventArgs(animation));
            }
        }
    }
}
