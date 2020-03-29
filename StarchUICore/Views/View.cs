using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Helpers;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Layers;
using System;

namespace StarchUICore.Views
{
    public class View : Element, IView
    {
        public Layer Foreground { get; set; }
        public Layer Background { get; set; }

        public override void Load() { }
        public override void Draw() { }

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
            /*var position = new Vector3(Location.X, Location.Y, 0.0f);
            var borderThickness = Border.Thickness;
            var size = new Vector2(Measurement.Width, Measurement.Height);
            var cornerRadius = new Vector2(Border.CornerXRadius, Border.CornerYRadius);
            var color = new Color4(Background.Color.R, Background.Color.G, Background.Color.B, Alpha);
            var borderColor = Border.Color;
            var selectionID = SelectionHelper.GetColorFromID(id);*/
            // Now that we have a new measurement for this view, we can add vertices for its drawable functionality!
            //var vertexSet = UIBuilder.Rectangle(Measurement.Width, Measurement.Height, new Color4(Background.Color.R, Background.Color.G, Background.Color.B, Alpha));

            // TODO - Begin applying all the desired transformations to the vertex set (such as textures)
            //_vertexSet = vertexSet;
            //_vertexBuffer.AddVertices(_vertexSet.Vertices);
            //_indexBuffer.AddIndices(_vertexSet.TriangleIndicesShort);
        }

        public virtual IView Duplicate() => throw new NotImplementedException();

        /*protected override void OnAlphaChanged(float oldValue, float newValue)
        {
            base.OnAlphaChanged(oldValue, newValue);

            _vertexSet.Update(v => v is IColorVertex colorVertex
                ? (ViewVertex)colorVertex.Colored(new Color4(colorVertex.Color.R, colorVertex.Color.G, colorVertex.Color.B, newValue))
                : v);
        }*/
    }
}
