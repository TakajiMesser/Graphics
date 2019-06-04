using SpiceEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public interface IMesh
    {
        IEnumerable<IVertex3D> Vertices { get; }
        float Alpha { get; set; }
        event EventHandler<AlphaEventArgs> AlphaChanged;
        void Load();
        void Draw();
        IMesh Duplicate();
    }
}
