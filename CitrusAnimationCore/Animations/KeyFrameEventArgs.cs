using System;

namespace CitrusAnimationCore.Animations
{
    public class KeyFrameEventArgs : EventArgs
    {
        public KeyFrameEventArgs(IKeyFrame keyFrame) => KeyFrame = keyFrame;

        public IKeyFrame KeyFrame { get; }
    }
}
