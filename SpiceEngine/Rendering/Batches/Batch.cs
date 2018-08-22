using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Rendering.Batches
{
    public class Batch<T> where T : IVertex3D
    {
        private MatrixStack _matrixStack;

        private VertexBuffer<T> _vertexBuffer;
        private VertexIndexBuffer _indexBuffer;
        private VertexArray<T> _vertexArray;

        public Batch() { }

        public void Add(IEnumerable<T> vertices)
        {

        }

        public void Load()
        {
            _matrixStack = new MatrixStack();

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
