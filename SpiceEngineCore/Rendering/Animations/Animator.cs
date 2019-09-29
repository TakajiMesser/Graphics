using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.Animations
{
    public class Animator
    {
        public List<Animation> Animations { get; private set; } = new List<Animation>();

        public Animation CurrentAnimation { get; set; }
        public float AnimationTick { get; private set; }

        public event EventHandler<KeyFrameEventArgs> Animate;
        public event EventHandler<IndexEventArgs> AnimationEnd;

        public void Tick()
        {
            if (CurrentAnimation == null) throw new InvalidOperationException("No current animation defined");

            // Find the corresponding keyframe for this tick
            var keyFrame = CurrentAnimation.KeyFrames.FirstOrDefault(k => k.Time == AnimationTick);
            if (keyFrame != null)
            {
                Animate?.Invoke(this, new KeyFrameEventArgs(keyFrame));
            }

            // For now, always repeat the animation upon completion
            AnimationTick += 1.0f;
            if (AnimationTick > CurrentAnimation.Duration)
            {
                AnimationTick = 0.0f;
                CurrentAnimation = null;
                AnimationEnd?.Invoke(this, new IndexEventArgs(0));
            }
        }
    }

    public class KeyFrameEventArgs : EventArgs
    {
        public KeyFrame KeyFrame { get; private set; }

        public KeyFrameEventArgs(KeyFrame keyFrame) => KeyFrame = keyFrame;
    }

    public class IndexEventArgs : EventArgs
    {
        public int Index { get; private set; }

        public IndexEventArgs(int index) => Index = index;
    }
}
