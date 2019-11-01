using SpiceEngineCore.Components;
using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderable : IComponent
    {
        bool IsAnimated { get; }
        bool IsTransparent { get; }

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
