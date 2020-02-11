using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Views;
using System;

namespace StarchUICore
{
    public abstract class Element : IElement
    {
        private Position _position;
        private Size _size;
        private float _alpha = 1.0f;

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

        public Size MinimumSize { get; set; } = Size.Auto();
        public Size MaximumSize { get; set; } = Size.Auto();

        public Padding Padding { get; set; } = Padding.Empty();
        public Anchor Anchor { get; set; } = Anchor.Default();

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

        public bool IsAnimated { get; set; } = false;
        public bool IsTransparent => Alpha < 1.0f;

        public event EventHandler<PositionEventArgs> PositionChanged;
        public event EventHandler<SizeEventArgs> SizeChanged;
        public event EventHandler<AlphaEventArgs> AlphaChanged;

        protected int ApplyMinimumWidthConstraint(int value, LayoutInfo layoutInfo) => MinimumSize.Width is AutoUnits
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
            : value.ClampTop(MaximumSize.Height.Constrain(layoutInfo.Size.Height, layoutInfo.Size.ContainingHeight));

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

            if (Measurement.NeedsMeasuring)
            {
                Measurement.SetValue(layoutResult.Width, layoutResult.Height);
            }

            if (Location.NeedsLocating)
            {
                Location.SetValue(layoutResult.X, layoutResult.Y);
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

        protected abstract LayoutResult OnLayout(LayoutInfo layoutInfo);
        protected abstract MeasuredSize OnMeasure(MeasuredSize availableSize);
        //protected abstract LocatedPosition OnLocate(LayoutInfo layoutInfo);
        protected abstract LocatedPosition OnLocate(LocatedPosition availablePosition);

        protected virtual void OnPositionChanged(Position oldValue, Position newValue) { }
        protected virtual void OnSizeChanged(Size oldValue, Size newValue) { }
        protected virtual void OnAlphaChanged(float oldValue, float newValue) { }
    }
}
