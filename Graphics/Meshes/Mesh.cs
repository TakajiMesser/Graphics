using Graphics.Lighting;
using Graphics.Materials;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Vertices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Meshes
{
    public class Mesh : IDisposable
    {
        private List<Vertex> _vertices = new List<Vertex>();
        private VertexArray<Vertex> _vertexArray;
        private VertexBuffer<Vertex> _vertexBuffer;
        private MaterialBuffer _materialBuffer;
        private LightBuffer _lightBuffer;
        private VertexIndexBuffer _indexBuffer;

        public IEnumerable<Vertex> Vertices => _vertices;

        public Mesh(List<Vertex> vertices, List<Material> materials, List<int> triangleIndices, ShaderProgram program)
        {
            if (triangleIndices.Count % 3 != 0)
            {
                throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");
            }

            _vertices = vertices;

            _indexBuffer = new VertexIndexBuffer();
            _indexBuffer.AddIndices(triangleIndices.ConvertAll(i => (ushort)i));

            _lightBuffer = new LightBuffer("lights", program);

            _materialBuffer = new MaterialBuffer("materials", program);
            _materialBuffer.AddMaterials(materials);

            _vertexBuffer = new VertexBuffer<Vertex>();
            _vertexBuffer.AddVertices(_vertices);

            _vertexArray = new VertexArray<Vertex>(_vertexBuffer, program);
        }

        public void AddTestColors()
        {
            for (var i = 0; i < _vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    _vertices[i] = new Vertex(_vertices[i].Position, _vertices[i].Normal, Color4.Lime, _vertices[i].TextureCoords, _vertices[i].MaterialIndex);
                }
                else if (i % 3 == 1)
                {
                    _vertices[i] = new Vertex(_vertices[i].Position, _vertices[i].Normal, Color4.Red, _vertices[i].TextureCoords, _vertices[i].MaterialIndex);
                }
                else if (i % 3 == 2)
                {
                    _vertices[i] = new Vertex(_vertices[i].Position, _vertices[i].Normal, Color4.Blue, _vertices[i].TextureCoords, _vertices[i].MaterialIndex);
                }
            }

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(_vertices);
        }

        public void AddTestLight()
        {
            _lightBuffer.Clear();
            _lightBuffer.AddLight(new Light()
            {
                Position = new Vector3(0.0f, 0.0f, 0.0f),
                Radius = 3.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.5f
            });
        }

        public void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _materialBuffer.Bind();
            _lightBuffer.Bind();
            _indexBuffer.Bind();

            _vertexBuffer.Buffer();
            _materialBuffer.Buffer();
            _lightBuffer.Buffer();
            _indexBuffer.Buffer();

            _indexBuffer.Draw();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
            _materialBuffer.Unbind();
            _lightBuffer.Unbind();
            _indexBuffer.Unbind();
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public void SaveToFile()
        {
            throw new NotImplementedException();
        }

        public static Mesh LoadFromFile(string path, ShaderProgram program)
        {
            var materials = new List<Material>();
            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var normals = new List<Vector3>();
            var vertexIndices = new List<int>();
            var uvIndices = new List<int>();
            var normalIndices = new List<int>();
            var materialIndices = new List<int>();

            var materialIndexByName = new Dictionary<string, int>();
            int currentMaterialIndex = 0;

            foreach (var line in File.ReadLines(path))
            {
                var values = line.Split(' ');

                if (values.Length > 0)
                {
                    switch (values[0])
                    {
                        case "mtllib":
                            foreach (var material in Material.LoadFromFile(Path.GetDirectoryName(path) + "\\" + values[1]))
                            {
                                materialIndexByName.Add(material.Item1, materials.Count);
                                materials.Add(material.Item2);
                            }
                            break;
                        case "usemtl":
                            if (!materialIndexByName.ContainsKey(values[1]))
                            {
                                throw new ArgumentOutOfRangeException("Material " + values[1] + " not found");
                            }
                            currentMaterialIndex = materialIndexByName[values[1]];
                            break;
                        case "v":
                            vertices.Add(new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])));
                            break;
                        case "vt":
                            uvs.Add(new Vector2(float.Parse(values[1]), float.Parse(values[2])));
                            break;
                        case "vn":
                            normals.Add(new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])));
                            break;
                        case "f":
                            for (var i = 1; i <= 3; i++)
                            {
                                var indices = values[i].Split('/');

                                vertexIndices.Add(int.Parse(indices[0]));
                                uvIndices.Add(int.Parse(indices[1]));
                                normalIndices.Add(int.Parse(indices[2]));
                                materialIndices.Add(currentMaterialIndex);
                            }
                            break;
                    }
                }
            }

            var verticies = new List<Vertex>();
            var triangleIndices = new List<int>();

            for (var i = 0; i < vertexIndices.Count; i++)
            {
                var meshVertex = new Vertex(vertices[vertexIndices[i]], normals[normalIndices[i]], uvs[uvIndices[i]], materialIndices[i]);
                var existingIndex = verticies.FindIndex(v => v.Position == meshVertex.Position
                    && v.Normal == meshVertex.Normal
                    && v.TextureCoords == meshVertex.TextureCoords
                    && v.MaterialIndex == meshVertex.MaterialIndex);

                if (existingIndex >= 0)
                {
                    triangleIndices.Add(existingIndex);
                }
                else
                {
                    triangleIndices.Add(verticies.Count);
                    verticies.Add(meshVertex);
                }
            }

            var mesh = new Mesh(verticies, materials, triangleIndices, program);

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

        ~Mesh()
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
