using System;

namespace CitrusAnimationCore.Animations
{
    public class KeyFrameEventArgs : EventArgs
    {
        public IKeyFrame KeyFrame { get; private set; }

        public KeyFrameEventArgs(IKeyFrame keyFrame) => KeyFrame = keyFrame;
    }
}
