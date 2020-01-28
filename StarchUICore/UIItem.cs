using SpiceEngineCore.Rendering;
using StarchUICore.Attributes;
using StarchUICore.Layers;
using StarchUICore.Views;
using System;

namespace StarchUICore
{
    public enum UIUnitTypes
    {
        Pixels,
        Percents
    }

    public abstract class UIItem : IUIItem
    {
        private Position _position;
        private float _alpha = 1.0f;

        public IUIItem Parent { get; set; }

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

        public Size Size { get; set; }

        public Measurement Measurement { get; protected set; } = Measurement.Empty;
        public bool IsMeasured { get; protected set; }

        public Border Border { get; set; }

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
        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public abstract void Load();
        public abstract void Measure(Size availableSize);
        public abstract void Update();
        public abstract void Draw();

        protected virtual void OnPositionChanged(Position oldValue, Position newValue) { }
        protected virtual void OnAlphaChanged(float oldValue, float newValue) { }
    }
}
