using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Layers;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public interface IView : IElement
    {
        Layer Foreground { get; set; }
        Layer Background { get; set; }

        IEnumerable<ViewQuadVertex> Vertices { get; }
        IEnumerable<int> TriangleIndices { get; }

        IView Duplicate();
    }
}
