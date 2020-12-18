using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Vertices;

namespace SweetGraphicsCore.Vertices
{
    public interface ISelectionVertex : IVertex
    {
        Color4 SelectionID { get; }

        ISelectionVertex Selected();
        ISelectionVertex Deselected();
    }
}
