using SpiceEngineCore.Rendering;
using StarchUICore.Attributes;
using StarchUICore.Layers;
using StarchUICore.Views;
using System;

namespace StarchUICore
{
    public interface IUIItem : IRenderable
    {
        IUIItem Parent { get; set; }

        Position Position { get; set; }
        Size Size { get; set; }

        Measurement Measurement { get; }
        bool IsMeasured { get; }

        Border Border { get; set; }

        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }
        bool IsGone { get; set; }

        event EventHandler<PositionEventArgs> PositionChanged;

        void Measure(Size availableSize);
        void Update();
    }
}
