using SpiceEngineCore.Geometry.Colors;
using SpiceEngineCore.Rendering.Vertices;

namespace SweetGraphicsCore.Vertices
{
    public interface ISelectionVertex : IVertex
    {
        Color4 IDColor { get; }

        ISelectionVertex Selected();
        ISelectionVertex Deselected();
    }
}
