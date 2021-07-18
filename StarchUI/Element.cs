using SpiceEngineCore.Components;
using SpiceEngineCore.Rendering;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using StarchUICore.Attributes.Units;
using StarchUICore.Traversal;
using System;
using System.Collections.Generic;

namespace StarchUICore
{
    public abstract class Element : Component, IElement
    {
        private Position _position;
        private Size _size;
        private float _alpha = 1.0f;

        public Element(int entityID) : base(entityID) { }

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

        public bool IsLaidOut => !Measurement.NeedsMeasuring;

        public bool IsTransparent => Alpha < 1.0f;
        public bool IsAnimated { get; set; } = false;
        public bool IsSelectable { get; set; } = true;

        public event EventHandler<LayoutEventArgs> MeasurementChanged;
        public event EventHandler<PositionEventArgs> PositionChanged;
        public event EventHandler<SizeEventArgs> SizeChanged;
        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public abstract void Load(IRenderContext renderContext);

        public virtual void Update(int nTicks) { }

        public abstract void Draw();

        public virtual void InvalidateMeasurement() => Measurement.Invalidate();

        public virtual void InvokeMeasurementChanged()
        {
            if (!Measurement.NeedsMeasuring)
            {
                OnMeasured();
                MeasurementChanged?.Invoke(this, new LayoutEventArgs(this));
            }
        }

        public void MeasureWidth(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsWidthMeasuring)
            {
                var width = GetMeasuredWidth(layoutInfo);
                Measurement.SetWidth(width);
                InvokeMeasurementChanged();
            }
        }

        public void MeasureHeight(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsHeightMeasuring)
            {
                var height = GetMeasuredHeight(layoutInfo);
                Measurement.SetHeight(height);
                InvokeMeasurementChanged();
            }
        }

        public void MeasureX(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsXMeasuring)
            {
                var relativeX = GetRelativeX(layoutInfo);
                var absoluteX = layoutInfo.ParentX + relativeX;
                Measurement.SetX(absoluteX);
                InvokeMeasurementChanged();
            }
        }

        public void MeasureY(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsYMeasuring)
            {
                var relativeY = GetRelativeY(layoutInfo);
                var absoluteY = layoutInfo.ParentY + relativeY;
                Measurement.SetY(absoluteY);
                InvokeMeasurementChanged();
            }
        }

        protected abstract int GetMeasuredWidth(LayoutInfo layoutInfo);
        protected abstract int GetMeasuredHeight(LayoutInfo layoutInfo);
        protected abstract int GetRelativeX(LayoutInfo layoutInfo);
        protected abstract int GetRelativeY(LayoutInfo layoutInfo);

        public virtual IEnumerable<LayoutDependency> GetXDependencies()
        {
            var anchorID = GetReferenceID(HorizontalAnchor.RelativeElement);

            if (!(Position.X is AutoUnits) || !(Position.MinimumX is AutoUnits) || !(Position.MaximumX is AutoUnits))
            {
                if (HorizontalAnchor.SelfAnchorType == AnchorTypes.Center || HorizontalAnchor.SelfAnchorType == AnchorTypes.End)
                {
                    yield return LayoutDependency.Width(EntityID);
                }

                if (anchorID.HasValue)
                {
                    yield return LayoutDependency.X(anchorID.Value);

                    if (HorizontalAnchor.RelativeAnchorType == AnchorTypes.Center || HorizontalAnchor.RelativeAnchorType == AnchorTypes.End
                        || Position.X is PercentUnits || Position.MinimumX is PercentUnits || Position.MaximumX is PercentUnits)
                    {
                        yield return LayoutDependency.Width(anchorID.Value);
                    }
                }
            }
        }

        public virtual IEnumerable<LayoutDependency> GetYDependencies()
        {
            var anchorID = GetReferenceID(VerticalAnchor.RelativeElement);

            if (!(Position.Y is AutoUnits) || !(Position.MinimumY is AutoUnits) || !(Position.MaximumY is AutoUnits))
            {
                if (VerticalAnchor.SelfAnchorType == AnchorTypes.Center || VerticalAnchor.SelfAnchorType == AnchorTypes.End)
                {
                    yield return LayoutDependency.Height(EntityID);
                }

                if (anchorID.HasValue)
                {
                    yield return LayoutDependency.Y(anchorID.Value);

                    if (VerticalAnchor.RelativeAnchorType == AnchorTypes.Center || VerticalAnchor.RelativeAnchorType == AnchorTypes.End
                        || Position.Y is PercentUnits || Position.MinimumY is PercentUnits || Position.MaximumY is PercentUnits)
                    {
                        yield return LayoutDependency.Height(anchorID.Value);
                    }
                }
            }
        }

        public virtual IEnumerable<LayoutDependency> GetWidthDependencies()
        {
            var dockID = GetReferenceID(HorizontalDock.RelativeElement);

            if (dockID.HasValue && (Size.Width is PercentUnits || Size.MinimumWidth is PercentUnits || Size.MaximumWidth is PercentUnits))
            {
                yield return LayoutDependency.Width(dockID.Value);
            }
        }

        public virtual IEnumerable<LayoutDependency> GetHeightDependencies()
        {
            var dockID = GetReferenceID(VerticalDock.RelativeElement);

            if (dockID.HasValue && (Size.Height is PercentUnits || Size.MinimumHeight is PercentUnits || Size.MaximumHeight is PercentUnits))
            {
                yield return LayoutDependency.Height(dockID.Value);
            }
        }

        private int? GetReferenceID(IElement relativeElement)
        {
            if (relativeElement != null)
            {
                return relativeElement.EntityID;
            }
            else if (Parent != null)
            {
                return Parent.EntityID;
            }

            return null;
        }

        protected virtual void OnMeasured() { }

        protected virtual void OnPositionChanged(Position oldValue, Position newValue) { }
        protected virtual void OnSizeChanged(Size oldValue, Size newValue) { }
        protected virtual void OnAlphaChanged(float oldValue, float newValue) { }
    }
}
