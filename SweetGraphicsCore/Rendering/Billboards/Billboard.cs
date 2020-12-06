using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Buffers;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using Color4 = SpiceEngineCore.Geometry.Colors.Color4;

namespace SweetGraphicsCore.Rendering.Billboards
{
    public class Billboard : IBillboard, IDisposable
    {
        private VertexArray<ColorVertex3D> _vertexArray = new VertexArray<ColorVertex3D>();
        private VertexBuffer<ColorVertex3D> _vertexBuffer = new VertexBuffer<ColorVertex3D>();

        private List<ColorVertex3D> _vertices = new List<ColorVertex3D>();
        private float _alpha = 1.0f;

        private string _textureFilePath;

        public Billboard(Vector3 position, string textureFilePath)
        {
            _textureFilePath = textureFilePath;
            _vertices.Add(new ColorVertex3D(position, Color4.White));
        }

        public IEnumerable<IVertex3D> Vertices => _vertices.Cast<IVertex3D>();
        public int TextureIndex { get; private set; }

        public float Alpha
        {
            get => _alpha;
            set
            {
                if (_alpha != value)
                {
                    var oldValue = _alpha;
                    _alpha = value;
                    AlphaChanged?.Invoke(this, new AlphaEventArgs(oldValue, value));
                }
            }
        }

        public bool IsTransparent => Alpha < 1.0f;
        public bool IsAnimated => false;
        public bool IsSelectable { get; set; } = true;

        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public void LoadTexture(ITextureProvider textureProvider) => TextureIndex = !string.IsNullOrEmpty(_textureFilePath) ? textureProvider.AddTexture(_textureFilePath) : -1;

        public void AddVertices(IEnumerable<ColorVertex3D> vertices) => _vertices.AddRange(vertices);
        public void ClearVertices() => _vertices.Clear();

        public void Combine(IBillboard billboard)
        {
            if (billboard is Billboard castBillboard)
            {
                _vertices.AddRange(castBillboard._vertices);
            }
        }

        public void AddVertex(Vector3 position, Color4 color)
        {
            _vertices.Add(new ColorVertex3D(position, color));
            _vertexBuffer.AddVertex(_vertices[_vertices.Count - 1]);
        }

        public void Transform(Transform transform) => Transform(transform, 0, _vertices.Count);
        public void Transform(Transform transform, int offset, int count)
        {
            for (var i = offset; i < count; i++)
            {
                //var originalVertex = _vertices[i];
                var transformedVertex = (ColorVertex3D)_vertices[i].Transformed(transform);
                _vertices[i] = transformedVertex;
            }

            // TODO - This is very redundant to keep two separate lists of vertex (struct) data
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(_vertices);
            }
        }

        public void SetColor(Color4 color) => SetColor(color, 0, _vertices.Count);
        public void SetColor(Color4 color, int offset, int count)
        {
            for (var i = offset; i < count; i++)
            {
                //var originalVertex = _vertices[i];
                var coloredVertex = (ColorVertex3D)_vertices[i].Colored(color);
                _vertices[i] = coloredVertex;
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
            _vertexBuffer = new VertexBuffer<ColorVertex3D>();
            _vertexArray = new VertexArray<ColorVertex3D>();

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(_vertices);

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

        public IBillboard Duplicate()
        {
            var billboard = new Billboard(_vertices.First().Position, _textureFilePath);

            for (var i = 1; i < _vertices.Count; i++)
            {
                billboard.AddVertex(_vertices[i].Position, _vertices[i].Color);
            }

            return billboard;
        }

        /*public TextureID Duplicate() => new TextureID(FilePath)
        {
            Index = Index,
            Alpha = Alpha
        };*/

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

        ~Billboard()
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
