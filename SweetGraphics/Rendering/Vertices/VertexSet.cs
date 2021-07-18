using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Vertices
{
    public class VertexSet<T> : IVertexSet where T : struct, IVertex
    {
        private List<T> _vertices = new List<T>();
        private List<int> _triangleIndices = new List<int>();

        private VertexBuffer<T> _vertexBuffer;
        private VertexArray<T> _vertexArray;
        private float _alpha = 1.0f;

        public IEnumerable<IVertex> Vertices
        {
            get
            {
                for (var i = 0; i < _vertexBuffer.Count; i++)
                {
                    yield return _vertexBuffer.GetVertex(i);
                }
            }
        }

        public float Alpha
        {
            get => _alpha;
            set
            {
                if (_alpha != value)
                {
                    Update(v => v is IColorVertex colorVertex
                        ? (T)colorVertex.Colored(new Color4(colorVertex.Color.R, colorVertex.Color.G, colorVertex.Color.B, value))
                        : v);

                    var oldValue = _alpha;
                    _alpha = value;
                    AlphaChanged?.Invoke(this, new AlphaEventArgs(oldValue, value));
                }
            }
        }

        public bool IsTransparent => Alpha < 1.0f;
        public bool IsAnimated => typeof(T) == typeof(AnimatedVertex3D);
        public bool IsSelectable { get; set; } = true;

        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public IVertexSet Duplicate() => new VertexSet<T>();

        public void Combine(IVertexSet vertexSet)
        {
            if (vertexSet is VertexSet<T> castVertexSet)
            {

            }
        }

        public void Transform(Transform transform) => Transform(transform, 0, _vertexBuffer.Count);
        public void Transform(Transform transform, int offset, int count)
        {
            var vertices = Vertices.ToList();

            _vertexBuffer.Clear();
            //_vertexBuffer.AddVertices(vertices.Select(v => (T)v.Transformed(transform)));
        }

        public void TransformTexture(Vector3 center, Vector2 translation, float rotation, Vector2 scale) => TransformTexture(center, translation, rotation, scale, 0, _vertexBuffer.Count);
        public void TransformTexture(Vector3 center, Vector2 translation, float rotation, Vector2 scale, int offset, int count)
        {
            var vertices = Vertices.ToList();

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(vertices.Select(v => v is ITextureVertex textureVertex
                ? (T)textureVertex.TextureTransformed(center, translation, rotation, scale)
                : (T)v));
        }

        public void Update(Func<IVertex, IVertex> vertexUpdate) => Update(vertexUpdate, 0, _vertexBuffer.Count);
        public void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count)
        {
            var vertices = Vertices.ToList();

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(vertices.Select(v => (T)vertexUpdate(v)));
        }

        public void Load(IRenderContext renderContext)
        {
            _vertexBuffer = new VertexBuffer<T>(renderContext);
            _vertexArray = new VertexArray<T>(renderContext);

            //_vertexBuffer.AddVertices(_vertexSet.Vertices);

            _vertexBuffer.Load();
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }

        public void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();

            _vertexBuffer.Buffer();
            GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }
    }
}
