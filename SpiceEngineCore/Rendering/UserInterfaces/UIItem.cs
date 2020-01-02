using SpiceEngineCore.Rendering.UserInterfaces.Attributes;
using SpiceEngineCore.Rendering.UserInterfaces.Layers;
using SpiceEngineCore.Rendering.UserInterfaces.Views;
using System;

namespace SpiceEngineCore.Rendering.UserInterfaces
{
    public abstract class UIItem : IUIItem
    {
        private float _alpha = 1.0f;

        public IUIItem Parent { get; set; }

        public Position Position { get; set; }
        public Size Size { get; set; }

        public UIBorder Border { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }

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

        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public abstract void Load();
        public abstract UIQuadSet Measure();
        public abstract void Update();
        public abstract void Draw();

        protected virtual void OnAlphaChanged(float oldValue, float newValue) { }
    }
}
