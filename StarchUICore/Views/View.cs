using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using SpiceEngineCore.Utilities;
using StarchUICore.Attributes;
using StarchUICore.Helpers;
using StarchUICore.Layers;
using System;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public class View : UIItem, IView
    {
        private Vertex3DSet<ViewVertex> _vertexSet = new Vertex3DSet<ViewVertex>();

        private VertexBuffer<ViewVertex> _vertexBuffer;
        private VertexIndexBuffer _indexBuffer;
        private VertexArray<ViewVertex> _vertexArray;

        public View() { }
        public View(Vertex3DSet<ViewVertex> vertexSet) => _vertexSet = vertexSet;

        public Layer Foreground { get; set; }
        public Layer Background { get; set; }

        public IEnumerable<ViewVertex> Vertices => _vertexSet.Vertices;
        public IEnumerable<int> TriangleIndices => _vertexSet.TriangleIndices;

        public void Combine(IView view)
        {
            if (view is View castView)
            {
                _vertexSet.Combine(castView._vertexSet);
            }
        }

        public void Transform(Transform transform) => Transform(transform, 0, _vertexSet.VertexCount);
        public void Transform(Transform transform, int offset, int count)
        {
            _vertexSet.Update(v => (ViewVertex)((IVertex3D)v).Transformed(transform), offset, count);

            // TODO - This is very redundant to keep two separate lists of vertex (struct) data
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(_vertexSet.Vertices);
            }
        }

        public void Update(Func<IVertex, IVertex> vertexUpdate) => Update(vertexUpdate, 0, _vertexSet.VertexCount);
        public void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count)
        {
            _vertexSet.Update(v => (ViewVertex)vertexUpdate(v), offset, count);

            // TODO - This is very redundant to keep two separate lists of vertex (struct) data
            if (_vertexBuffer != null)
            {
                _vertexBuffer.Clear();
                _vertexBuffer.AddVertices(_vertexSet.Vertices);
            }
        }

        public override void Load()
        {
            _vertexBuffer = new VertexBuffer<ViewVertex>();
            _indexBuffer = new VertexIndexBuffer();
            _vertexArray = new VertexArray<ViewVertex>();

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(_vertexSet.Vertices);
            _indexBuffer.AddIndices(_vertexSet.TriangleIndicesShort);

            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }

        public override void Measure(Size availableSize)
        {
            if (!IsMeasured)
            {
                var width = 0;
                var height = 0;

                if (!IsGone)
                {
                    if (Size.WidthUnits.IsAbsolute() && availableSize.WidthUnits.IsAbsolute())
                    {
                        width = Size.Width.Clamp(0, availableSize.Width);
                    }
                    else if (!Size.WidthUnits.IsAbsolute() && availableSize.WidthUnits.IsAbsolute())
                    {
                        width = (int)(Size.Width * 0.01f * availableSize.Width);
                    }
                    else if (Size.WidthUnits.IsAbsolute() && !availableSize.WidthUnits.IsAbsolute())
                    {
                        // TODO - This shouldn't happen, the parent MUST be measured absolutely before the child
                    }
                    else if (!Size.WidthUnits.IsAbsolute() && !availableSize.WidthUnits.IsAbsolute())
                    {
                        // TODO - This means that we are taking a percent of a percent?
                    }

                    if (Size.HeightUnits.IsAbsolute() && availableSize.HeightUnits.IsAbsolute())
                    {
                        height = Size.Height.Clamp(0, availableSize.Height);
                    }
                    else if (!Size.HeightUnits.IsAbsolute() && availableSize.HeightUnits.IsAbsolute())
                    {
                        height = (int)(Size.Height * 0.01f * availableSize.Height);
                    }
                    else if (Size.HeightUnits.IsAbsolute() && !availableSize.HeightUnits.IsAbsolute())
                    {
                        // TODO - This shouldn't happen, the parent MUST be measured absolutely before the child
                    }
                    else if (!Size.HeightUnits.IsAbsolute() && !availableSize.HeightUnits.IsAbsolute())
                    {
                        // TODO - This means that we are taking a percent of a percent?
                    }
                }

                Measurement = new Measurement(width, height);
                IsMeasured = true;
            }
        }

        public override void Update()
        {
            if (IsEnabled)
            {

            }
        }

        public override void Draw()
        {
            if (IsVisible && Measurement.Width > 0 && Measurement.Height > 0)
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
        }

        public virtual IView Duplicate() => new View();

        protected override void OnAlphaChanged(float oldValue, float newValue)
        {
            base.OnAlphaChanged(oldValue, newValue);

            _vertexSet.Update(v => v is IColorVertex colorVertex
                ? (ViewVertex)colorVertex.Colored(new Color4(colorVertex.Color.R, colorVertex.Color.G, colorVertex.Color.B, newValue))
                : v);
        }
    }
}
