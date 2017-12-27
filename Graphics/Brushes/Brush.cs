using Graphics.Lighting;
using Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using OpenTK.Graphics;
using OpenTK;
using Graphics.Matrices;
using Graphics.Physics.Collision;
using Graphics.Helpers;
using Graphics.Rendering.Vertices;
using Graphics.GameObjects;

namespace Graphics.Brushes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush
    {
        internal ShaderProgram _program;
        private ModelMatrix _modelMatrix = new ModelMatrix();

        private List<Vertex> _vertices = new List<Vertex>();
        private VertexArray<Vertex> _vertexArray;
        private VertexBuffer<Vertex> _vertexBuffer;
        private MaterialBuffer _materialBuffer;
        private LightBuffer _lightBuffer;
        private VertexIndexBuffer _indexBuffer;
        private ICollider _collider;

        public List<Vertex> Vertices => _vertices;
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public ICollider Collider
        {
            get => _collider;
            set
            {
                _collider = value;
                _collider.Properties = Properties;
            }
        }

        public Brush(List<Vertex> vertices, List<Material> materials, List<int> triangleIndices, ShaderProgram program)
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
            _program = program;
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
                Radius = 30.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.5f
            });
            _lightBuffer.AddLight(new Light()
            {
                Position = new Vector3(0.0f, 20.0f, 0.0f),
                Radius = 30.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.25f
            });
        }

        public void OnRenderFrame()
        {
            _modelMatrix.Set(_program);
            Draw();
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
        }

        public static Brush Rectangle(Vector3 center, float width, float height, ShaderProgram program)
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector2.Zero, 0)
            };

            var materials = new List<Material>
            {
                Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
            };

            var triangleIndices = new List<int>()
            {
                0, 1, 2, 1, 2, 3
            };

            return new Brush(vertices, materials, triangleIndices, program);
        }
    }
}
