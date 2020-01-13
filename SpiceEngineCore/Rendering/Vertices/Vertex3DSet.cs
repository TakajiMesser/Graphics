using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.Vertices
{
    public class Vertex3DSet<T> where T : IVertex3D
    {
        private List<T> _vertices = new List<T>();
        private List<int> _triangleIndices = new List<int>();

        public Vertex3DSet() { }
        public Vertex3DSet(IList<T> vertices, IList<int> triangleIndices)
        {
            if (triangleIndices.Count % 3 != 0) throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");

            _vertices.AddRange(vertices);
            _triangleIndices.AddRange(triangleIndices);
        }

        public IEnumerable<T> Vertices => _vertices;
        public IEnumerable<int> TriangleIndices => _triangleIndices;
        public IEnumerable<ushort> TriangleIndicesShort => _triangleIndices.ConvertAll(i => (ushort)i);

        public int VertexCount => _vertices.Count;
        public int IndexCount => _triangleIndices.Count;

        public Vertex3DSet<T> Duplicate() => new Vertex3DSet<T>(_vertices.ToList(), _triangleIndices.ToList());

        public Vertex3DSet<T> Updated(Func<IVertex3D, IVertex3D> vertexUpdate)
        {
            var updatedVertices = new List<T>();

            for (var i = 0; i < _vertices.Count; i++)
            {
                var updatedVertex = vertexUpdate(_vertices[i]);
                updatedVertices[i] = (T)updatedVertex;
            }

            return new Vertex3DSet<T>(updatedVertices, _triangleIndices.ToList());
        }

        public void AddVertices(IEnumerable<T> vertices) => _vertices.AddRange(vertices);
        public void ClearVertices() => _vertices.Clear();

        public void Combine(Vertex3DSet<T> vertexSet)
        {
            var offset = _vertices.Count;
            _vertices.AddRange(vertexSet._vertices);
            _triangleIndices.AddRange(vertexSet._triangleIndices.Select(i => i + offset));
        }

        public void Update(Func<IVertex, IVertex> vertexUpdate) => Update(vertexUpdate, 0, _vertices.Count);
        public void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count)
        {
            for (var i = offset; i < count; i++)
            {
                var updatedVertex = vertexUpdate(_vertices[i]);
                _vertices[i] = (T)updatedVertex;
            }
        }

        public void Clear()
        {
            _vertices.Clear();
            _triangleIndices.Clear();
        }
    }
}
