using SpiceEngineCore.Rendering;
using SpiceEngineCore.UserInterfaces;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using StarchUICore.Traversal;
using System;
using System.Collections.Generic;

namespace StarchUICore
{
    public interface IElement : IUIElement, IRenderable
    {
        int EntityID { get; }
        string Name { get; set; }
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

        event EventHandler<PositionEventArgs> PositionChanged;
        event EventHandler<SizeEventArgs> SizeChanged;
        event EventHandler<LayoutEventArgs> MeasurementChanged;

        IEnumerable<LayoutDependency> GetXDependencies();
        IEnumerable<LayoutDependency> GetYDependencies();
        IEnumerable<LayoutDependency> GetWidthDependencies();
        IEnumerable<LayoutDependency> GetHeightDependencies();

        void MeasureX(LayoutInfo layoutInfo);
        void MeasureY(LayoutInfo layoutInfo);
        void MeasureWidth(LayoutInfo layoutInfo);
        void MeasureHeight(LayoutInfo layoutInfo);

        void InvokeMeasurementChanged();
    }
}
