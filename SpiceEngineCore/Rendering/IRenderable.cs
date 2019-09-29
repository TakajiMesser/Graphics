using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderable
    {
        bool IsAnimated { get; }
        bool IsTransparent { get; }

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
