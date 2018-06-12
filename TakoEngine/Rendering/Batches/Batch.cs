using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Vertices;

namespace TakoEngine.Rendering.Batches
{
    public class Batch<T> where T : struct, IVertex
    {
        private VertexBuffer<T> _vertexBuffer;
        private VertexIndexBuffer _indexBuffer;
        private VertexArray<T> _vertexArray;

        public Batch()
        {

        }

        public void Add(IEnumerable<T> vertices)
        {

        }
    }
}
