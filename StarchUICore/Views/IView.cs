using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Layers;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public interface IView : IElement
    {
        Layer Foreground { get; set; }
        Layer Background { get; set; }

        IEnumerable<ViewVertex> Vertices { get; }
        IEnumerable<int> TriangleIndices { get; }

        IView Duplicate();
    }
}
