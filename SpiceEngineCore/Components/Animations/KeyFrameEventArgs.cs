using SpiceEngineCore.Components.Animations;
using System;

namespace SpiceEngineCore.Components.Animations
{
    public class KeyFrameEventArgs : EventArgs
    {
        public IKeyFrame KeyFrame { get; private set; }

        public KeyFrameEventArgs(IKeyFrame keyFrame) => KeyFrame = keyFrame;
    }
}
