using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.UserInterfaces.Groups
{
    public class UIGroup : IUIGroup, IDisposable
    {
        public IEnumerable<ViewVertex> Vertices => _vertices;
        public IEnumerable<int> TriangleIndices => _triangleIndices;
        public float Alpha
        {
            get => _alpha;
            set
            {
                if (_alpha != value)
                {
                    for (var i = 0; i < _vertices.Count; i++)
                    {
                        var vertex = _vertices[i];
                        _vertices[i] = (ViewVertex)vertex.Colored(new Color4(vertex.Color.R, vertex.Color.G, vertex.Color.B, value));
                    }

                    var oldValue = _alpha;
                    _alpha = value;
                    AlphaChanged?.Invoke(this, new AlphaEventArgs(oldValue, value));
                }
            }
        }

        public bool IsAnimated => false;
        public bool IsTransparent => Alpha < 1.0f;

        public event EventHandler<AlphaEventArgs> AlphaChanged;

        private List<ViewVertex> _vertices = new List<ViewVertex>();
        private List<int> _triangleIndices = new List<int>();

        private VertexBuffer<ViewVertex> _vertexBuffer;
        private VertexIndexBuffer _indexBuffer;
        private VertexArray<ViewVertex> _vertexArray;

        private float _alpha = 1.0f;

        public UIGroup(List<ViewVertex> vertices, List<int> triangleIndices)
        {
            if (triangleIndices.Count % 3 != 0)
            {
                throw new ArgumentException(nameof(triangleIndices) + " must be divisible by three");
            }

            _vertices.AddRange(vertices);
            _triangleIndices.AddRange(triangleIndices);
        }

        public IUIGroup Duplicate() => new UIGroup(_vertices.ToList(), _triangleIndices.ToList());

        public void AddVertices(IEnumerable<ViewVertex> vertices) => _vertices.AddRange(vertices);
        public void ClearVertices() => _vertices.Clear();

        public void Combine(IUIGroup group)
        {
            if (group is UIGroup castGroup)
            {
                var offset = _vertices.Count;
                _vertices.AddRange(castGroup._vertices);
                _triangleIndices.AddRange(castGroup._triangleIndices.Select(i => i + offset));
            }
        }

        public void Transform(Transform transform) => Transform(transform, 0, _vertices.Count);
        public void Transform(Transform transform, int offset, int count)
        {
            for (var i = offset; i < count; i++)
            {
                //var originalVertex = _vertices[i];
                var transformedVertex = (ViewVertex)_vertices[i].Transformed(transform);
                _vertices[i] = transformedVertex;
            }

            // TODO - This is very redundant to keep two separate lists of vertex (struct) data
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(_vertices);
            }
        }

        public void Update(Func<IVertex, IVertex> vertexUpdate) => Update(vertexUpdate, 0, _vertices.Count);
        public void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count)
        {
            for (var i = offset; i < count; i++)
            {
                var updatedVertex = vertexUpdate(_vertices[i]);

                /*if (updatedVertex is EditorVertex3D editorVertex)
                {
                    _vertices[i] = (T)editorVertex.Colored(new Color4(1.0f, 0.0f, 0.0f, 1.0f));
                }
                else
                {*/
                _vertices[i] = (ViewVertex)updatedVertex;
                //}
            }

            // TODO - This is very redundant to keep two separate lists of vertex (struct) data
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(_vertices);
            }
        }

        public void Load()
        {
            _vertexBuffer = new VertexBuffer<ViewVertex>();
            _indexBuffer = new VertexIndexBuffer();
            _vertexArray = new VertexArray<ViewVertex>();

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(_vertices);
            _indexBuffer.AddIndices(_triangleIndices.ConvertAll(i => (ushort)i));

            _vertexBuffer.Bind();
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

        public virtual void SaveToFile()
        {
            throw new NotImplementedException();
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

        ~UIGroup()
        {
            Dispose(false);
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
