using SpiceEngineCore.Rendering;
using SpiceEngineCore.UserInterfaces;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using System;

namespace StarchUICore
{
    public interface IElement : IUIElement, IRenderable
    {
        int EntityID { get; set; }
        string Name { get; set; }
        int TabCount { get; set; }
        IElement Parent { get; set; }

        Position Position { get; set; }
        Size Size { get; set; }

        Anchor HorizontalAnchor { get; set; }
        Anchor VerticalAnchor { get; set; }

        Dock HorizontalDock { get; set; }
        Dock VerticalDock { get; set; }

        Padding Padding { get; set; }
        Border Border { get; set; }

        Measurement Measurement { get; }

        event EventHandler<LayoutEventArgs> LayoutChanged;
        //event EventHandler<EventArgs> Draw;
        event EventHandler<PositionEventArgs> PositionChanged;
        event EventHandler<SizeEventArgs> SizeChanged;

        void MeasureX(LayoutInfo layoutInfo);
        void MeasureY(LayoutInfo layoutInfo);
        void MeasureWidth(LayoutInfo layoutInfo);
        void MeasureHeight(LayoutInfo layoutInfo);
        void Layout(LayoutInfo layoutInfo);

        void ApplyCorrections(int widthChange, int heightChange, int xChange, int yChange);
        void InvokeLayoutChange();
    }
}
