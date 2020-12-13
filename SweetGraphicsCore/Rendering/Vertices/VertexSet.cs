using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Vertices
{
    public class VertexSet<T> : IVertexSet, IDisposable where T : IVertex3D
    {
        private VertexBuffer<T> _vertexBuffer;
        private VertexArray<T> _vertexArray;
        private float _alpha = 1.0f;

        public VertexSet()
        {

        }

        public IEnumerable<IVertex3D> Vertices
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
            _vertexBuffer.AddVertices(vertices.Select(v => (T)v.Transformed(transform)));
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

        public void Load()
        {
            _vertexBuffer = new VertexBuffer<T>();
            _vertexArray = new VertexArray<T>();

            _vertexBuffer.Clear();
            //_vertexBuffer.AddVertices(_vertexSet.Vertices);

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

        ~VertexSet()
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
