using SpiceEngineCore.Rendering.UserInterfaces.Attributes;
using SpiceEngineCore.Rendering.UserInterfaces.Layers;
using SpiceEngineCore.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.UserInterfaces.Views
{
    public interface IUIView : IRenderable
    {
        IUIView Parent { get; set; }

        Position Position { get; set; }
        Size Size { get; set; }

        UILayer Foreground { get; set; }
        UILayer Background { get; set; }

        IEnumerable<ViewVertex> Vertices { get; }
        IEnumerable<int> TriangleIndices { get; }
        float Alpha { get; set; }

        void AddVertices(IEnumerable<ViewVertex> vertices);
        void ClearVertices();

        void Measure();
        void Update();

        IUIView Duplicate();
    }
}
