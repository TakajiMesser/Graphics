using SpiceEngineCore.Components;
using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderable : IComponent
    {
        bool IsAnimated { get; }
        bool IsTransparent { get; }

        void Load();
        void Draw();

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
