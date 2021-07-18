using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderable
    {
        bool IsTransparent { get; }
        bool IsAnimated { get; }
        bool IsSelectable { get; }

        void Load(IRenderContext renderContext);
        void Draw();

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
