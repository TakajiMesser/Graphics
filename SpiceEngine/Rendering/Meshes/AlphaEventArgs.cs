using System;

namespace SpiceEngine.Rendering.Meshes
{
    public class AlphaEventArgs : EventArgs
    {
        public float OldAlpha { get; }
        public float NewAlpha { get; }

        public bool IsTransparent => NewAlpha < 1.0f;
        public bool WasTransparent => OldAlpha < 1.0f;

        public AlphaEventArgs(float oldAlpha, float newAlpha)
        {
            OldAlpha = oldAlpha;
            NewAlpha = newAlpha;
        }
    }
}
