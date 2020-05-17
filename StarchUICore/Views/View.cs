using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
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

        protected override int GetRelativeX(LayoutInfo layoutInfo)
        {
            var anchorWidth = HorizontalAnchor.GetReferenceWidth(layoutInfo);

            // Apply our Position attribute to achieve this element's desired X
            var relativeX = Position.X.ToOffsetPixels(layoutInfo.AvailableValue, anchorWidth);

            // Pass this desired relative X back to the Anchor to reposition it appropriately
            if (!(Position.X is AutoUnits))
            {
                relativeX = HorizontalAnchor.GetAnchorX(relativeX, Measurement, layoutInfo);
            }

            // Apply the Minimum and Maximum constraints last, as these are HARD requirements
            relativeX = Position.MinimumX.ConstrainAsMinimum(relativeX, anchorWidth);
            relativeX = Position.MaximumX.ConstrainAsMaximum(relativeX, anchorWidth);

            return relativeX;
        }

        protected override int GetRelativeY(LayoutInfo layoutInfo)
        {
            var anchorHeight = VerticalAnchor.GetReferenceHeight(layoutInfo);

            // Apply our Position attribute to achieve this element's desired Y
            var relativeY = Position.Y.ToOffsetPixels(layoutInfo.AvailableValue, anchorHeight);

            // Pass this desired relative Y back to the Anchor to reposition it appropriately
            if (!(Position.Y is AutoUnits))
            {
                relativeY = VerticalAnchor.GetAnchorY(relativeY, Measurement, layoutInfo);
            }

            // Apply the Minimum and Maximum constraints last, as these are HARD requirements
            relativeY = Position.MinimumY.ConstrainAsMinimum(relativeY, anchorHeight);
            relativeY = Position.MaximumY.ConstrainAsMaximum(relativeY, anchorHeight);

            return relativeY;
        }

        protected override int GetMeasuredWidth(LayoutInfo layoutInfo)
        {
            var dockWidth = HorizontalDock.GetReferenceWidth(layoutInfo);
            var constrainedWidth = Size.Width.ToDimensionPixels(layoutInfo.AvailableValue, dockWidth);

            constrainedWidth = Size.MinimumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);
            constrainedWidth = Size.MaximumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);

            return constrainedWidth;
        }

        protected override int GetMeasuredHeight(LayoutInfo layoutInfo)
        {
            var dockHeight = VerticalDock.GetReferenceHeight(layoutInfo);
            var constrainedHeight = Size.Height.ToDimensionPixels(layoutInfo.AvailableValue, dockHeight);

            constrainedHeight = Size.MinimumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);
            constrainedHeight = Size.MaximumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);

            return constrainedHeight;
        }

        public override void Update(int nTicks)
        {
            if (IsEnabled)
            {

            }
        }

        protected override void OnMeasured()
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
