using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Styling;
using StarchUICore.Attributes.Units;
using StarchUICore.Views;
using System;
using System.Collections.Generic;
using System.Text;

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

        public bool IsTransparent => Alpha < 1.0f;
        public bool IsAnimated { get; set; } = false;
        public bool IsSelectable { get; set; } = true;

        public event EventHandler<LayoutEventArgs> LayoutChanged;
        public event EventHandler<PositionEventArgs> PositionChanged;
        public event EventHandler<SizeEventArgs> SizeChanged;
        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public abstract void Load();
        //public abstract void Measure(ISize availableSize);
        //public abstract void Locate(IPosition availablePosition);

        public virtual void Update(int nTicks) { }

        public abstract void Draw();

        public virtual void InvalidateMeasurement()
        {
            Measurement.Invalidate();
            Location.Invalidate();
        }

        public virtual void InvalidateLocation() => Location.Invalidate();

        public void Layout(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsMeasuring || Location.NeedsLocating)
            {
                Log();
                Log(layoutInfo);
                _layoutInfo = layoutInfo;
                var layoutResult = OnLayout(layoutInfo);
                Log(layoutResult);

                if (Measurement.NeedsMeasuring && layoutResult.Width.HasValue && layoutResult.Height.HasValue)
                {
                    Measurement.SetValue(layoutResult.Width.Value, layoutResult.Height.Value);
                    OnMeasured(layoutInfo);
                }

                if (Location.NeedsLocating && layoutResult.X.HasValue && layoutResult.Y.HasValue)
                {
                    Location.SetValue(layoutResult.X.Value, layoutResult.Y.Value);
                    OnLocated(layoutInfo);
                }

                OnLaidOut(layoutInfo);
                LayoutChanged?.Invoke(this, new LayoutEventArgs(this));
            }
        }

        private LayoutInfo? _layoutInfo;

        public virtual void ApplyCorrections(int widthChange, int heightChange, int xChange, int yChange)
        {
            // We need to redock AND reanchor :/
            /*if (_layoutInfo.HasValue)
            {
                var width = GetMeasuredWidth(_layoutInfo.Value.AvailableWidth, _layoutInfo.Value.ParentWidth);
                var height = GetMeasuredHeight(_layoutInfo.Value.AvailableHeight, _layoutInfo.Value.ParentHeight);

                if (width.HasValue && height.HasValue)
                {
                    Measurement.SetValue(width.Value, height.Value);
                }

                var relativeX = GetRelativeX(_layoutInfo.Value.RelativeX, _layoutInfo.Value.ParentAbsoluteX, _layoutInfo.Value.AvailableWidth, _layoutInfo.Value.ParentWidth, width);
                var relativeY = GetRelativeY(_layoutInfo.Value.RelativeY, _layoutInfo.Value.ParentAbsoluteY, _layoutInfo.Value.AvailableHeight, _layoutInfo.Value.ParentHeight, height);

                var absoluteX = GetAbsoluteX(_layoutInfo.Value.ParentAbsoluteX, relativeX, width);
                var absoluteY = GetAbsoluteY(_layoutInfo.Value.ParentAbsoluteY, relativeY, height);

                if (absoluteX.HasValue && absoluteY.HasValue)
                {
                    Location.SetValue(absoluteX.Value, absoluteY.Value);
                }
            }
            else
            {
                Measurement.SetValue(Measurement.Width + widthChange, Measurement.Height + heightChange);
                Location.SetValue(Location.X + xChange, Location.Y + yChange);
            }*/

            var width = Measurement.Width;// + widthChange;
            var height = Measurement.Height;// + heightChange;
            var x = Location.X + xChange;
            var y = Location.Y + yChange;

            if (HorizontalDock.RelativeElement == null)
            {
                
            }

            if (VerticalDock.RelativeElement == null)
            {

            }

            if (HorizontalAnchor.RelativeElement == null)
            {
                if (HorizontalAnchor.RelativeAnchorType == AnchorTypes.Start)
                {
                    
                }
                else if (HorizontalAnchor.RelativeAnchorType == AnchorTypes.Center)
                {
                    x += widthChange / 2;
                }
                else if (HorizontalAnchor.RelativeAnchorType == AnchorTypes.End)
                {
                    x += widthChange;
                }
            }

            if (VerticalAnchor.RelativeElement == null)
            {
                if (VerticalAnchor.RelativeAnchorType == AnchorTypes.Start)
                {

                }
                else if (VerticalAnchor.RelativeAnchorType == AnchorTypes.Center)
                {
                    y += heightChange / 2;
                }
                else if (VerticalAnchor.RelativeAnchorType == AnchorTypes.End)
                {
                    y += heightChange;
                }
            }

            Measurement.SetValue(width, height);
            Location.SetValue(x, y);

            // TODO - This is truly terrible...
            if (this is Label label)
            {
                label.Text = label.Text;
            }

            LayoutChanged?.Invoke(this, new LayoutEventArgs(this));
        }

        public virtual void InvokeLayoutChange() => LayoutChanged?.Invoke(this, new LayoutEventArgs(this));

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

            // TODO - Parent should suggest Y, but give availability bounds in BOTH directions for this Element to use as needed
            var minParentY = suggestedRelativeY;
            var maxParentY = suggestedRelativeY + availableHeight;

            // First, determine what this element thinks its Y should be
            var constrainedY = Position.GetConstrainedY(suggestedRelativeY, referenceHeight);

            if (constrainedY.HasValue && measuredHeight.HasValue)
            {
                var anchoredY = VerticalAnchor.GetAnchorY(constrainedY.Value, measuredHeight.Value, minParentY, maxParentY, parentHeight, parentAbsoluteY);

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

        // TODO - For debugging purposes
        public int TabCount { get; set; } = 0;

        private void PrependTabs(StringBuilder builder, int nTabs)
        {
            for (var i = 0; i < nTabs; i++)
            {
                builder.Append("\t");
            }
        }

        protected void Log(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
        }

        protected string GetNameLine()
        {
            var builder = new StringBuilder();

            PrependTabs(builder, TabCount);
            builder.Append("[" + Name + "]");

            return builder.ToString();
        }

        protected string GetPhaseLine(string phaseName)
        {
            var builder = new StringBuilder();

            PrependTabs(builder, TabCount + 1);
            builder.Append(phaseName);

            return builder.ToString();
        }

        protected string GetValueLine(string name, string value)
        {
            var builder = new StringBuilder();

            PrependTabs(builder, TabCount + 2);
            builder.Append(name + " = " + value);

            return builder.ToString();
        }

        protected string GetValueLine(string name, int value)
        {
            var builder = new StringBuilder();

            PrependTabs(builder, TabCount + 2);
            builder.Append(name + " = " + value);

            return builder.ToString();
        }

        protected string GetValueLine(string name, IUnits value)
        {
            var builder = new StringBuilder();

            PrependTabs(builder, TabCount + 2);
            builder.Append(name + " = ");

            switch (value)
            {
                case PixelUnits pixelUnits:
                    builder.Append(pixelUnits.Value + "px");
                    break;
                case PercentUnits percentUnits:
                    builder.Append(percentUnits.Value + "%");
                    break;
                case AutoUnits _:
                    builder.Append("AUTO");
                    break;
            }

            return builder.ToString();
        }

        protected string GetValueLine(string name, int? value)
        {
            var builder = new StringBuilder();

            PrependTabs(builder, TabCount + 2);
            builder.Append(name + " = " + (value.HasValue ? value.Value.ToString() : "NULL"));

            return builder.ToString();
        }

        protected void Log()
        {
            Log(GetNameLine().Yield());

            Log(new List<string>
            {
                GetPhaseLine("Position"),
                GetValueLine("X", Position.X),
                GetValueLine("Y", Position.Y),
                /*GetValueLine("Min X", Position.MinimumX),
                GetValueLine("Min Y", Position.MinimumY),
                GetValueLine("Max X", Position.MaximumX),
                GetValueLine("Max Y", Position.MaximumY)*/
            });

            Log(new List<string>
            {
                GetPhaseLine("Size"),
                GetValueLine("Width", Size.Width),
                GetValueLine("Height", Size.Height),
                /*GetValueLine("Min Width", Size.MinimumWidth),
                GetValueLine("Min Height", Size.MinimumHeight),
                GetValueLine("Max Width", Size.MaximumWidth),
                GetValueLine("Max Height", Size.MaximumHeight)*/
            });

            Log(new List<string>
            {
                GetPhaseLine("Anchor"),
                GetValueLine("Horizontal Type", HorizontalAnchor.SelfAnchorType.ToString()),
                //GetValueLine("Horizontal Relative", HorizontalAnchor.RelativeElement != null ? HorizontalAnchor.RelativeElement.Name : "PARENT"),
                GetValueLine("Vertical Type", VerticalAnchor.SelfAnchorType.ToString()),
                //GetValueLine("Vertical Relative", VerticalAnchor.RelativeElement != null ? VerticalAnchor.RelativeElement.Name : "PARENT")
            });

            /*Log(new List<string>
            {
                GetPhaseLine("Dock"),
                GetValueLine("Horizontal Relative", HorizontalDock.RelativeElement != null ? HorizontalDock.RelativeElement.Name : "PARENT"),
                GetValueLine("Vertical Relative", VerticalDock.RelativeElement != null ? VerticalDock.RelativeElement.Name : "PARENT")
            });*/
        }

        protected void Log(LayoutInfo layoutInfo)
        {
            var lines = new List<string>
            {
                GetPhaseLine("On Entry"),
                GetValueLine("AvailableWidth", layoutInfo.AvailableWidth),
                GetValueLine("AvailableHeight", layoutInfo.AvailableHeight),
                GetValueLine("ParentWidth", layoutInfo.ParentWidth),
                GetValueLine("ParentHeight", layoutInfo.ParentHeight),
                GetValueLine("RelativeX", layoutInfo.RelativeX),
                GetValueLine("RelativeY", layoutInfo.RelativeY),
                GetValueLine("ParentAbsoluteX", layoutInfo.ParentAbsoluteX),
                GetValueLine("ParentAbsoluteY", layoutInfo.ParentAbsoluteY)
            };

            Log(lines);
        }

        protected void Log(LayoutResult layoutResult)
        {
            var lines = new List<string>
            {
                GetPhaseLine("On Exit"),
                GetValueLine("X", layoutResult.X),
                GetValueLine("Y", layoutResult.Y),
                GetValueLine("Width", layoutResult.Width),
                GetValueLine("Height", layoutResult.Height)
            };

            Log(lines);
        }

        protected void Log(int? width, int? height, int? relativeX, int? relativeY, int? absoluteX, int? absoluteY)
        {
            var lines = new List<string>
            {
                GetPhaseLine("On Self"),
                GetValueLine("width", width),
                GetValueLine("height", height),
                GetValueLine("relativeX", relativeX),
                GetValueLine("relativeY", relativeY),
                GetValueLine("absoluteX", absoluteX),
                GetValueLine("absoluteY", absoluteY)
            };

            Log(lines);
        }
    }
}
