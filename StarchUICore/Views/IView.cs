using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Layers;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public interface IView : IUIItem
    {
        Layer Foreground { get; set; }
        Layer Background { get; set; }

        IEnumerable<ViewVertex> Vertices { get; }
        IEnumerable<int> TriangleIndices { get; }
        float Alpha { get; set; }

        IView Duplicate();
    }
}
