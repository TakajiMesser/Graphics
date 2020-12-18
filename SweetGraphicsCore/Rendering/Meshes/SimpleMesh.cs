using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public class SimpleMesh : IDisposable
    {
        private int _vertexArrayHandle;
        private VertexBuffer<Simple3DVertex> _vertexBuffer = new VertexBuffer<Simple3DVertex>();
        private VertexIndexBuffer _indexBuffer = new VertexIndexBuffer();

        public SimpleMesh(List<Vector3> vertices, List<int> triangleIndices, ShaderProgram program)
        {
            if (triangleIndices.Count % 3 != 0) throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");

            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);

            _vertexBuffer.AddVertices(vertices.Select(v => new Simple3DVertex(v)));
            _vertexBuffer.Bind();
            //_vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(program.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();

            _indexBuffer.AddIndices(triangleIndices.ConvertAll(i => (ushort)i));
        }
        
        public void Draw()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();
            _indexBuffer.Bind();

            _vertexBuffer.Buffer();
            _indexBuffer.Buffer();

            _indexBuffer.Draw();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
            _indexBuffer.Unbind();
        }

        public static SimpleMesh LoadFromFile(string path, ShaderProgram program)
        {
            var vertices = new List<Vector3>();
            var vertexIndices = new List<int>();

            foreach (var line in File.ReadLines(path))
            {
                var values = line.Split(' ');

                if (values.Length > 0)
                {
                    switch (values[0])
                    {
                        case "v":
                            vertices.Add(new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])));
                            break;
                        case "f":
                            for (var i = 1; i <= 3; i++)
                            {
                                var indices = values[i].Split('/');
                                vertexIndices.Add(int.Parse(indices[0]) - 1);
                            }
                            break;
                    }
                }
            }

            var verticies = new List<Vector3>();
            var triangleIndices = new List<int>();

            for (var i = 0; i < vertexIndices.Count; i++)
            {
                var vertex = vertices[vertexIndices[i]];
                var existingIndex = verticies.FindIndex(v => v == vertex);

                if (existingIndex >= 0)
                {
                    triangleIndices.Add(existingIndex);
                }
                else
                {
                    triangleIndices.Add(verticies.Count);
                    verticies.Add(vertex);
                }
            }

            var mesh = new SimpleMesh(verticies, triangleIndices, program);

            return mesh;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                //GL.DeleteShader(Handle);
                disposedValue = true;
            }
        }

        ~SimpleMesh()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
