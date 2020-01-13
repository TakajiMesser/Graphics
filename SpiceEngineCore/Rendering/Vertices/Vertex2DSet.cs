using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Matrices;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Rendering.Vertices
{
    public class Vertex2DSet<T> where T : IVertex2D
    {
        public List<T> Vertices { get; set; }
        public List<int> TriangleIndices { get; set; }
    }
}
