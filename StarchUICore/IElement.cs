using SpiceEngineCore.Rendering;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Views;
using System;

namespace StarchUICore
{
    public interface IElement : IRenderable
    {
        IElement Parent { get; set; }

        Position Position { get; set; }
        Size Size { get; set; }

        Border Border { get; set; }

        Measurement Measurement { get; }
        Location Location { get; }

        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }
        bool IsGone { get; set; }

        float Alpha { get; set; }

        event EventHandler<PositionEventArgs> PositionChanged;
        event EventHandler<SizeEventArgs> SizeChanged;

        void Layout(LayoutInfo layoutInfo);
        void Measure(MeasuredSize availableSize);
        void Locate(LocatedPosition availablePosition);
        void Update(int nTicks);
    }
}
