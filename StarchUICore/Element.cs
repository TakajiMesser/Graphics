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

        public int EntityID { get; set; }
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

        public event EventHandler<LayoutEventArgs> LayoutChanged;
        public event EventHandler<PositionEventArgs> PositionChanged;
        public event EventHandler<SizeEventArgs> SizeChanged;
        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public abstract void Load();
        //public abstract void Measure(ISize availableSize);
        //public abstract void Locate(IPosition availablePosition);

        public virtual void Update(int nTicks) { }

        public abstract void Draw();

        public virtual void InvalidateMeasurement() => Measurement.Invalidate();

        public void Layout(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsMeasuring)
            {
                Log();
                Log(layoutInfo);
                //_layoutInfo = layoutInfo;
                var layoutResult = OnLayout(layoutInfo);
                Log(layoutResult);

                OnLaidOut(layoutInfo);
                LayoutChanged?.Invoke(this, new LayoutEventArgs(this));
            }
        }

        //private LayoutInfo? _layoutInfo;

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
            var x = Measurement.X + xChange;
            var y = Measurement.Y + yChange;

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

            Measurement.SetValue(x, y, width, height);

            // TODO - This is truly terrible...
            if (this is Label label)
            {
                label.Text = label.Text;
            }

            LayoutChanged?.Invoke(this, new LayoutEventArgs(this));
        }

        public virtual void InvokeLayoutChange() => LayoutChanged?.Invoke(this, new LayoutEventArgs(this));

        public void PropageShit()
        {
            if (!Measurement.NeedsMeasuring)
            {
                LayoutChanged?.Invoke(this, new LayoutEventArgs(this));
            }
        }

        public void MeasureWidth(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsWidthMeasuring)
            {
                var width = GetMeasuredWidth(layoutInfo);
                Measurement.SetWidth(width);
                PropageShit();
            }
        }

        public void MeasureHeight(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsHeightMeasuring)
            {
                var height = GetMeasuredHeight(layoutInfo);
                Measurement.SetHeight(height);
                PropageShit();
            }
        }

        public void MeasureX(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsXMeasuring)
            {
                var relativeX = GetRelativeX(layoutInfo);
                var absoluteX = layoutInfo.ParentX + relativeX;
                Measurement.SetX(absoluteX);
                PropageShit();
            }
        }

        public void MeasureY(LayoutInfo layoutInfo)
        {
            if (Measurement.NeedsYMeasuring)
            {
                var relativeY = GetRelativeY(layoutInfo);
                var absoluteY = layoutInfo.ParentY + relativeY;
                Measurement.SetY(absoluteY);
                PropageShit();
            }
        }

        protected abstract int GetMeasuredWidth(LayoutInfo layoutInfo);
        protected abstract int GetMeasuredHeight(LayoutInfo layoutInfo);
        protected abstract int GetRelativeX(LayoutInfo layoutInfo);
        protected abstract int GetRelativeY(LayoutInfo layoutInfo);

        protected abstract LayoutResult OnLayout(LayoutInfo layoutInfo);

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
                GetValueLine("AvailableValue", layoutInfo.AvailableValue),
                GetValueLine("ParentX", layoutInfo.ParentX),
                GetValueLine("ParentY", layoutInfo.ParentY),
                GetValueLine("ParentWidth", layoutInfo.ParentWidth),
                GetValueLine("ParentHeight", layoutInfo.ParentHeight),
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
