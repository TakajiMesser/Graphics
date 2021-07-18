using System;

namespace SpiceEngineCore.Rendering
{
    public class ResolutionEventArgs : EventArgs
    {
        public ResolutionEventArgs(Resolution resolution) => Resolution = resolution;

        public Resolution Resolution { get; }
    }
}
