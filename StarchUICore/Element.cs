using SpiceEngineCore.Rendering;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
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

        public abstract void Load();
        //public abstract void Measure(ISize availableSize);
        //public abstract void Locate(IPosition availablePosition);
        public abstract void Update();
        public abstract void Draw();

        public virtual void InvalidateMeasurement()
        {
            Measurement.Invalidate();
            Location.Invalidate();
        }

        public virtual void InvalidateLocation() => Location.Invalidate();

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
                Location.SetValue(locatedPosition.X, locatedPosition.Y);
            }
        }

        protected abstract MeasuredSize OnMeasure(MeasuredSize availableSize);
        protected abstract LocatedPosition OnLocate(LocatedPosition availablePosition);

        protected virtual void OnPositionChanged(Position oldValue, Position newValue) { }
        protected virtual void OnSizeChanged(Size oldValue, Size newValue) { }
        protected virtual void OnAlphaChanged(float oldValue, float newValue) { }
    }
}
