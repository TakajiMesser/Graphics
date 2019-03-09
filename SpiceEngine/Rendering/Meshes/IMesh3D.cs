using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public interface IMesh3D
    {
        IEnumerable<IVertex3D> Vertices { get; }
        float Alpha { get; set; }
        void Load();
        void Draw();
        IMesh3D Duplicate();
    }
}
