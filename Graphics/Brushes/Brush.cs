using Graphics.Lighting;
using Graphics.Materials;
using Graphics.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Graphics.Brushes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush
    {
        private List<Vertex> _vertices = new List<Vertex>();
        private VertexArray<Vertex> _vertexArray;
        private VertexBuffer<Vertex> _vertexBuffer;
        private MaterialBuffer _materialBuffer;
        private LightBuffer _lightBuffer;
        private VertexIndexBuffer _indexBuffer;

        public void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _materialBuffer.Bind();
            _lightBuffer.Bind();
            _indexBuffer.Bind();

            _vertexBuffer.Buffer();
            //_materialBuffer.Set();
            _materialBuffer.Buffer();
            _lightBuffer.Buffer();
            _indexBuffer.Buffer();

            _vertexBuffer.Draw();
            _indexBuffer.Draw();

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }
    }
}
