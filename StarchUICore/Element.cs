using SpiceEngineCore.Rendering;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using System;

namespace StarchUICore
{
    public abstract class Element : IElement
    {
        private Position _position;
        private Size _size;
        private float _alpha = 1.0f;

        public string Name { get; set; }
        public IElement Parent { get; set; }

        public Position Position
        {
            get => _position;
            set
            {
                if (!_position.Equals(value))
                {
                    var oldValue = _position;
                    _position = value;

                    OnPositionChanged(oldValue, value);
                    PositionChanged?.Invoke(this, new PositionEventArgs(oldValue, value));
                }
            }
        }

        public Size Size
        {
            get => _size;
            set
            {
                if (!_size.Equals(value))
                {
                    var oldValue = _size;
                    _size = value;

                    OnSizeChanged(oldValue, value);
                    SizeChanged?.Invoke(this, new SizeEventArgs(oldValue, value));
                }
            }
        }

        public Anchor HorizontalAnchor { get; set; }
        public Anchor VerticalAnchor { get; set; }

        public Dock HorizontalDock { get; set; }
        public Dock VerticalDock { get; set; }

        public Padding Padding { get; set; } = Padding.Empty();
        public Border Border { get; set; }

        public Measurement Measurement { get; protected set; } = Measurement.Empty;
        public Location Location { get; protected set; } = Location.Empty;

        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public bool IsGone { get; set; } = false;

        public float Alpha
        {
            get => _alpha;
            set
            {
                if (_alpha != value)
                {
                    var oldValue = _alpha;
                    _alpha = value;

                    OnAlphaChanged(oldValue, value);
                    AlphaChanged?.Invoke(this, new AlphaEventArgs(oldValue, value));
                }
            }
        }

        public bool IsLaidOut => !Measurement.NeedsMeasuring && !Location.NeedsLocating;

        public bool IsAnimated { get; set; } = false;
        public bool IsTransparent => Alpha < 1.0f;

        public event EventHandler<LayoutEventArgs> LayoutChanged;
        public event EventHandler<PositionEventArgs> PositionChanged;
        public event EventHandler<SizeEventArgs> SizeChanged;
        public event EventHandler<AlphaEventArgs> AlphaChanged;

        /*protected int ApplyMinimumWidthConstraint(int value, LayoutInfo layoutInfo) => MinimumSize.Width is AutoUnits
            ? value
            : value.ClampBottom(MinimumSize.Width.Constrain(layoutInfo.Size.Width, layoutInfo.Size.ContainingWidth));

        protected int ApplyMaximumWidthConstraint(int value, LayoutInfo layoutInfo) => MaximumSize.Width is AutoUnits
            ? value
            : value.ClampTop(MaximumSize.Width.Constrain(layoutInfo.Size.Width, layoutInfo.Size.ContainingWidth));

        protected int ApplyMinimumHeightConstraint(int value, LayoutInfo layoutInfo) => MinimumSize.Height is AutoUnits
            ? value
            : value.ClampBottom(MinimumSize.Height.Constrain(layoutInfo.Size.Height, layoutInfo.Size.ContainingHeight));

        protected int ApplyMaximumHeightConstraint(int value, LayoutInfo layoutInfo) => MaximumSize.Height is AutoUnits
            ? value
            : value.ClampTop(MaximumSize.Height.Constrain(layoutInfo.Size.Height, layoutInfo.Size.ContainingHeight));*/

        public abstract void Load();
        //public abstract void Measure(ISize availableSize);
        //public abstract void Locate(IPosition availablePosition);

        public virtual void Update(int nTicks)
        {

        }

        public abstract void Draw();

        public virtual void InvalidateMeasurement()
        {
            Measurement.Invalidate();
            Location.Invalidate();
        }

        public virtual void InvalidateLocation() => Location.Invalidate();

        public void Layout(LayoutInfo layoutInfo)
        {
            var layoutResult = OnLayout(layoutInfo);
            
            var wasMeasured = false;
            var wasLocated = false;

            if (Measurement.NeedsMeasuring && layoutResult.Width.HasValue && layoutResult.Height.HasValue)
            {
                Measurement.SetValue(layoutResult.Width.Value, layoutResult.Height.Value);
                OnMeasured(layoutInfo);
                wasMeasured = true;
            }

            if (Location.NeedsLocating && layoutResult.X.HasValue && layoutResult.Y.HasValue)
            {
                Location.SetValue(layoutResult.X.Value, layoutResult.Y.Value);
                OnLocated(layoutInfo);
                wasLocated = true;
            }

            if (wasMeasured || wasLocated)
            {
                OnLaidOut(layoutInfo);
                LayoutChanged?.Invoke(this, new LayoutEventArgs(this));
            }
        }

        public void Measure(MeasuredSize availableSize)
        {
            if (Measurement.NeedsMeasuring)
            {
                var measuredSize = OnMeasure(availableSize);
                Measurement.SetValue(measuredSize.Width, measuredSize.Height);
            }
        }

        public void Locate(LocatedPosition availablePosition)
        {
            if (Location.NeedsLocating)
            {
                var locatedPosition = OnLocate(availablePosition);
                Location.SetValue(locatedPosition.AbsoluteX, locatedPosition.AbsoluteY);
            }
        }

        protected int? GetMeasuredWidth(int availableWidth, int parentWidth)
        {
            var dockWidth = HorizontalDock.GetReferenceWidth(parentWidth);
            return Size.ConstrainWidth(availableWidth, dockWidth);
        }

        protected int? GetMeasuredHeight(int availableHeight, int parentHeight)
        {
            var dockHeight = VerticalDock.GetReferenceHeight(parentHeight);
            return Size.ConstrainHeight(availableHeight, dockHeight);
        }

        /* The parent has very clearly outlined for us the following:
            - Here is my absolute X
            - Here is the X, RELATIVE TO MY LEFT SIDE, that I think this element should be placed at
            - If you should disagree, here is how much remaining width I have that you can work with
            - If you need my width as reference, or your own measured width, here they are

            This Element is now tasked with reporting back what actual Relative X it is going with
            It will do this by performing the following:
            - Determine if the suggested relative X is acceptable
        */
        protected int? GetRelativeX(int suggestedRelativeX, int parentAbsoluteX, int availableWidth, int parentWidth, int? measuredWidth)
        {
            var referenceWidth = HorizontalAnchor.GetAnchoredWidth(parentWidth);

            // TODO - Parent should suggest X, but give availability bounds in BOTH directions for this Element to use as needed
            var minParentX = suggestedRelativeX;
            var maxParentX = suggestedRelativeX + availableWidth;

            // First, determine what this element thinks its X should be
            var constrainedX = Position.GetConstrainedX(suggestedRelativeX, referenceWidth);

            if (constrainedX.HasValue && measuredWidth.HasValue)
            {
                var anchoredX = HorizontalAnchor.GetAnchorX(constrainedX.Value, measuredWidth.Value, minParentX, maxParentX, parentWidth, parentAbsoluteX);

                if (anchoredX.HasValue)
                {
                    return anchoredX;
                }
            }

            return null;
            
            /*var anchoredX = HorizontalAnchor.GetAnchoredX(relativeX, parentAbsoluteX, availableWidth, measuredWidth);
            var anchorWidth = HorizontalAnchor.GetAnchoredWidth(parentWidth);
            return Position.ConstrainX(availableWidth, measuredWidth, anchoredX, anchorWidth);*/
        }

        protected int? GetRelativeY(int suggestedRelativeY, int parentAbsoluteY, int availableHeight, int parentHeight, int? measuredHeight)
        {
            var referenceHeight = VerticalAnchor.GetAnchoredWidth(parentHeight);

            // TODO - Parent should suggest X, but give availability bounds in BOTH directions for this Element to use as needed
            var minParentX = suggestedRelativeY;
            var maxParentX = suggestedRelativeY + availableHeight;

            // First, determine what this element thinks its X should be
            var constrainedY = Position.GetConstrainedY(suggestedRelativeY, referenceHeight);

            if (constrainedY.HasValue && measuredHeight.HasValue)
            {
                var anchoredY = VerticalAnchor.GetAnchorY(constrainedY.Value, measuredHeight.Value, minParentX, maxParentX, parentHeight, parentAbsoluteY);

                if (anchoredY.HasValue)
                {
                    return anchoredY;
                }
            }

            return null;

            /*var anchorY = VerticalAnchor.GetAnchoredY(relativeY, parentAbsoluteY, measuredHeight);
            var anchorHeight = VerticalAnchor.GetAnchoredHeight(parentHeight);
            return Position.ConstrainY(availableHeight, measuredHeight, anchorY, anchorHeight);*/
        }

        protected int? GetAbsoluteX(int parentAbsoluteX, int? relativeX, int? measuredWidth)
        {
            // relativeX is what this view reported its internal X should be, but we ALSO need to consider the relative X that the parent passed us
            if (relativeX.HasValue)
            {
                return parentAbsoluteX + relativeX.Value;
                // TODO - Handle Centered Horizontal Anchor
                /*if (HorizontalAnchor.AnchorType == AnchorTypes.Start)
                {
                    return parentAbsoluteX + relativeX.Value;
                }
                else if (HorizontalAnchor.AnchorType == AnchorTypes.End)
                {
                    if (measuredWidth.HasValue)
                    {
                        return parentAbsoluteX + measuredWidth.Value - relativeX.Value;
                    }
                }*/
            }

            return null;
        }

        protected int? GetAbsoluteY(int parentAbsoluteY, int? relativeY, int? measuredHeight)
        {
            if (relativeY.HasValue)
            {
                return parentAbsoluteY + relativeY.Value;
                // TODO - Handle Centered Vertical Anchor
                /*if (VerticalAnchor.AnchorType == AnchorTypes.Start)
                {
                    return parentAbsoluteY + relativeY.Value;
                }
                else if (VerticalAnchor.AnchorType == AnchorTypes.End)
                {
                    if (measuredHeight.HasValue)
                    {
                        return parentAbsoluteY + measuredHeight.Value - relativeY.Value;
                    }
                }*/
            }

            return null;
        }

        protected abstract LayoutResult OnLayout(LayoutInfo layoutInfo);
        
        // TODO - Are these unnecessary now?
        protected abstract MeasuredSize OnMeasure(MeasuredSize availableSize);
        protected abstract LocatedPosition OnLocate(LocatedPosition availablePosition);

        protected virtual void OnMeasured(LayoutInfo layoutInfo) { }
        protected virtual void OnLocated(LayoutInfo layoutInfo) { }
        protected virtual void OnLaidOut(LayoutInfo layoutInfo) { }

        protected virtual void OnPositionChanged(Position oldValue, Position newValue) { }
        protected virtual void OnSizeChanged(Size oldValue, Size newValue) { }
        protected virtual void OnAlphaChanged(float oldValue, float newValue) { }
    }
}
