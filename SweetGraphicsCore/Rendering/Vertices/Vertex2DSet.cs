using System.Collections.Generic;

namespace SweetGraphicsCore.Vertices
{
    public class Vertex2DSet<T> where T : IVertex2D
    {
        public List<T> Vertices { get; set; }
        public List<int> TriangleIndices { get; set; }
    }
}
