using SpiceEngineCore.Rendering.Animations;
using System;

namespace SpiceEngineCore.Components.Animations
{
    public class KeyFrameEventArgs : EventArgs
    {
        public KeyFrame KeyFrame { get; private set; }

        public KeyFrameEventArgs(KeyFrame keyFrame) => KeyFrame = keyFrame;
    }
}
