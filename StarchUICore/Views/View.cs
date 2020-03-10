using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Builders;
using StarchUICore.Layers;
using System;
using System.Collections.Generic;

namespace StarchUICore.Views
{
    public class View : Element, IView
    {
        private Vertex3DSet<ViewQuadVertex> _vertexSet = new Vertex3DSet<ViewQuadVertex>();

        private VertexBuffer<ViewQuadVertex> _vertexBuffer;
        private VertexIndexBuffer _indexBuffer;
        private VertexArray<ViewQuadVertex> _vertexArray;

        // TODO - These should also be IUnits...
        public float CornerXRadius { get; set; } = 100.0f;
        public float CornerYRadius { get; set; } = 100.0f;

        public Layer Foreground { get; set; }
        public Layer Background { get; set; }

        public IEnumerable<ViewQuadVertex> Vertices => _vertexSet.Vertices;
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
            _vertexBuffer = new VertexBuffer<ViewQuadVertex>();
            _indexBuffer = new VertexIndexBuffer();
            _vertexArray = new VertexArray<ViewQuadVertex>();

            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(_vertexSet.Vertices);
            _indexBuffer.AddIndices(_vertexSet.TriangleIndicesShort);

            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }

        protected override LayoutResult OnLayout(LayoutInfo layoutInfo)
        {
            var width = GetMeasuredWidth(layoutInfo.AvailableWidth, layoutInfo.ParentWidth);
            var height = GetMeasuredHeight(layoutInfo.AvailableHeight, layoutInfo.ParentHeight);

            var relativeX = GetRelativeX(layoutInfo.RelativeX, layoutInfo.ParentAbsoluteX, layoutInfo.AvailableWidth, layoutInfo.ParentWidth, width);
            var relativeY = GetRelativeY(layoutInfo.RelativeY, layoutInfo.ParentAbsoluteY, layoutInfo.AvailableHeight, layoutInfo.ParentHeight, height);

            var absoluteX = GetAbsoluteX(layoutInfo.ParentAbsoluteX, relativeX, width);
            var absoluteY = GetAbsoluteY(layoutInfo.ParentAbsoluteY, relativeY, height);

            return new LayoutResult(absoluteX, absoluteY, width, height);
        }

        protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            /*var width = Size.Width.Constrain(availableSize.Width, availableSize.ContainingWidth);
            var height = Size.Height.Constrain(availableSize.Height, availableSize.ContainingHeight);

            return new MeasuredSize(width, height);*/
            return new MeasuredSize();
        }

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition)
        {
            throw new NotImplementedException();
        }

        /*protected override LocatedPosition OnLocate(LayoutInfo layoutInfo)
        {
            var relativeX = Position.X.GetValue(layoutInfo.Size.ContainingWidth);
            var relativeY = Position.Y.GetValue(layoutInfo.Size.ContainingHeight);

            var absoluteX = layoutInfo.Position.AbsoluteX + relativeX;
            var absoluteY = layoutInfo.Position.AbsoluteY + relativeY;

            return new LocatedPosition(absoluteX, absoluteY);
        }*/

        public override void Update(int nTicks)
        {
            if (IsEnabled)
            {

            }
        }

        protected override void OnMeasured(LayoutInfo layoutInfo)
        {
            /*// Now that we have a new measurement for this view, we can add vertices for its drawable functionality!
            var vertexSet = UIBuilder.Rectangle(Measurement.Width, Measurement.Height, new Color4(Background.Color.R, Background.Color.G, Background.Color.B, Alpha));

            // TODO - Begin applying all the desired transformations to the vertex set (such as textures)
            _vertexSet = vertexSet;
            _vertexBuffer.Clear();
            _vertexBuffer.AddVertices(_vertexSet.Vertices);
            _indexBuffer.AddIndices(_vertexSet.TriangleIndicesShort);*/
        }

        protected override void OnLaidOut(LayoutInfo layoutInfo)
        {
            _vertexBuffer.Clear();

            // Now that we have a new measurement for this view, we can add vertices for its drawable functionality!
            var vertexSet = UIBuilder.Rectangle(Measurement.Width, Measurement.Height, new Color4(Background.Color.R, Background.Color.G, Background.Color.B, Alpha));

            // TODO - Begin applying all the desired transformations to the vertex set (such as textures)
            _vertexSet = vertexSet;
            //_vertexBuffer.AddVertices(_vertexSet.Vertices);
            _indexBuffer.AddIndices(_vertexSet.TriangleIndicesShort);

            var position = new Vector3(400, 200, 0);//Location.X, Location.Y, 0.0f);
            var size = new Vector2(500, 400);//Measurement.Width, Measurement.Height);
            var cornerRadius = new Vector2(10, 10);//CornerXRadius, CornerYRadius);
            var color = new Color4(Background.Color.R, Background.Color.G, Background.Color.B, Alpha);
            
            _vertexBuffer.AddVertex(new ViewQuadVertex(position, size, cornerRadius, color, Color4.AliceBlue));
        }

        public override void Draw()
        {
            if (IsVisible && Measurement.Width > 0 && Measurement.Height > 0)
            {
                _vertexArray.Bind();
                _vertexBuffer.Bind();
                //_indexBuffer.Bind();

                _vertexBuffer.Buffer();
                //_indexBuffer.Buffer();

                //_indexBuffer.Draw();
                GL.DrawArrays(PrimitiveType.Points, 0, _vertexBuffer.Count);

                _vertexArray.Unbind();
                _vertexBuffer.Unbind();
                //_indexBuffer.Unbind();
            }
        }

        public virtual IView Duplicate() => throw new NotImplementedException();

        protected override void OnAlphaChanged(float oldValue, float newValue)
        {
            base.OnAlphaChanged(oldValue, newValue);

            _vertexSet.Update(v => v is IColorVertex colorVertex
                ? (ViewVertex)colorVertex.Colored(new Color4(colorVertex.Color.R, colorVertex.Color.G, colorVertex.Color.B, newValue))
                : v);
        }
    }
}
