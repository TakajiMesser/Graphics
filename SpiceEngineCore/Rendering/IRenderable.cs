using System;

namespace SpiceEngineCore.Rendering
{
    public interface IRenderable
    {
        bool IsTransparent { get; }
        bool IsAnimated { get; }
        bool IsSelectable { get; }

        void Load(IRenderContextProvider contextProvider);
        void Draw();

        event EventHandler<AlphaEventArgs> AlphaChanged;
    }
}
