using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Rendering.Batches
{
    public class Batch<T> where T : IVertex
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

        public void Load()
        {
            _vertexBuffer = new VertexBuffer<T>();
            _indexBuffer = new VertexIndexBuffer();
            _vertexArray = new VertexArray<T>();

            //_vertexBuffer.AddVertices(Vertices);
            //_indexBuffer.AddIndices(_triangleIndices.ConvertAll(i => (ushort)i));

            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();

            //_lightBuffer.Load(program);
        }

        public void Draw(/*ShaderProgram program, TextureManager textureManager = null*/)
        {
            /*if (textureManager != null && TextureMapping != null)
            {
                program.BindTextures(textureManager, TextureMapping);
            }
            else
            {
                program.UnbindTextures();
            }*/

            //_material.Draw(program);

            _vertexArray.Bind();
            _vertexBuffer.Bind();
            //_lightBuffer.Bind();
            _indexBuffer.Bind();

            _vertexBuffer.Buffer();
            //_lightBuffer.Buffer();
            _indexBuffer.Buffer();

            _indexBuffer.Draw();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
            //_lightBuffer.Unbind();
            _indexBuffer.Unbind();
        }
    }
}
