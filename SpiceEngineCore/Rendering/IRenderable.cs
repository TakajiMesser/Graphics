using SpiceEngineCore.Components;
using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderable : IComponent
    {
        bool IsTransparent { get; }
        bool IsAnimated { get; }
        bool IsSelectable { get; }

        void Load();
        void Draw();

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
