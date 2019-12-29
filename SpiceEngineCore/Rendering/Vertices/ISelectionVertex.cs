using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Matrices;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Rendering.Vertices
{
    public interface ISelectionVertex : IVertex
    {
        Color4 SelectionID { get; }

        ISelectionVertex Selected();
        ISelectionVertex Deselected();
    }
}
