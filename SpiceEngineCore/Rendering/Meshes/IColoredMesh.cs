using OpenTK.Graphics;
using System;

namespace SpiceEngineCore.Rendering.Meshes
{
    public interface IColoredMesh : IMesh
    {
        Color4 Color { get; set; }

        event EventHandler<ColorEventArgs> ColorChanged;
    }
}
