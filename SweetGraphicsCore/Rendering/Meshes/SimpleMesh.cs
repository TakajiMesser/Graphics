using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Shaders;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public class SimpleMesh
    {
        private VertexArray<Simple3DVertex> _vertexArray;
        private VertexBuffer<Simple3DVertex> _vertexBuffer;
        private VertexIndexBuffer _indexBuffer;

        public SimpleMesh(IRenderContext renderContext, List<Vector3> vertices, List<int> triangleIndices, ShaderProgram program)
        {
            if (triangleIndices.Count % 3 != 0) throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");

            _vertexBuffer = new VertexBuffer<Simple3DVertex>(renderContext);
            _indexBuffer = new VertexIndexBuffer(renderContext);
            _vertexArray = new VertexArray<Simple3DVertex>(renderContext);

            _vertexBuffer.AddVertices(vertices.Select(v => new Simple3DVertex(v)));
            _indexBuffer.AddIndices(triangleIndices.ConvertAll(i => (ushort)i));

            //var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            //attribute.Set(program.GetAttributeLocation("vPosition"));

            _vertexBuffer.Load();
            _vertexBuffer.Bind();
            _indexBuffer.Load();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }
        
        public void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();
            _indexBuffer.Bind();

            _vertexBuffer.Buffer();
            _indexBuffer.Buffer();

            _indexBuffer.Draw();

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
            _indexBuffer.Unbind();
        }

        public static SimpleMesh LoadFromFile(IRenderContext renderContext, string path, ShaderProgram program)
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

            return new SimpleMesh(renderContext, verticies, triangleIndices, program);
        }
    }
}
