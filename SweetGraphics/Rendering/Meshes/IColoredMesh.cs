using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using System;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public interface IColoredMesh : IMesh
    {
        Color4 Color { get; set; }

        event EventHandler<ColorEventArgs> ColorChanged;
    }
}
