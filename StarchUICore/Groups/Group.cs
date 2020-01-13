using SpiceEngineCore.Entities;
using SpiceEngineCore.Utilities;
using StarchUICore.Attributes;
using StarchUICore.Helpers;
using StarchUICore.Layers;
using System;
using System.Collections.Generic;

namespace StarchUICore.Groups
{
    public enum UILayoutTypes
    {
        Wrap,
        Fill
    }

    public class Group : UIItem, IGroup
    {
        private List<IUIItem> _children = new List<IUIItem>();

        public UILayoutTypes LayoutType { get; set; }

        public IEnumerable<IUIItem> GetChildren() => _children;

        public void AddChild(IUIItem item)
        {
            _children.Add(item);
            item.Parent = this;
        }

        public override void Load()
        {
            foreach (var child in GetChildren())
            {
                child.Load();
            }
        }

        public override void Measure(Size availableSize)
        {
            if (!IsMeasured)
            {
                var width = 0;
                var height = 0;

                if (!IsGone)
                {
                    var availableWidth = availableSize.Width;
                    var availableHeight = availableSize.Height;

                    if (Size.WidthUnits.IsAbsolute() && Size.Width < availableWidth)
                    {
                        availableWidth = Size.Width;
                    }

                    if (Size.HeightUnits.IsAbsolute() && Size.Height < availableHeight)
                    {
                        availableHeight = Size.Height;
                    }

                    foreach (var child in GetChildren())
                    {
                        child.Measure(new Size(availableWidth, availableHeight));

                        availableWidth -= child.Measurement.Width;
                        availableHeight -= child.Measurement.Height;

                        width += child.Measurement.Width;
                        height += child.Measurement.Height;
                    }

                    if (LayoutType == UILayoutTypes.Fill)
                    {
                        width = availableSize.Width;
                        height = availableSize.Height;
                    }
                }

                Measurement = new Measurement(width, height);
                IsMeasured = true;
            }
        }

        public override void Update()
        {
            if (IsEnabled)
            {
                foreach (var child in GetChildren())
                {
                    child.Update();
                }
            }
        }

        public override void Draw()
        {
            if (IsVisible && Measurement.Width > 0 && Measurement.Height > 0)
            {
                foreach (var child in GetChildren())
                {
                    child.Draw();
                }
            }
            // TODO - First, draw any group stuff (e.g. background, border, etc.)
            // Then, go through children and draw each one based on their previously measured sizes
        }

        public virtual IGroup Duplicate() => new Group();
    }
}
