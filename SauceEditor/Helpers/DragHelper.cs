using System;
using System.Collections.Generic;
using System.Windows;
using SpiceEngineCore.Utilities;
using System.Windows.Media;
using System.Linq;
using SauceEditor.Utilities;
using System.Windows.Controls;

namespace SauceEditor.Helpers
{
    public static class DragHelper
    {
        public static bool IsSignificantDrag(Vector vector) => Math.Abs(vector.X) > SystemParameters.MinimumHorizontalDragDistance
            || Math.Abs(vector.Y) > SystemParameters.MinimumVerticalDragDistance;

        public static int GetDropIndex(IList<UIElement> items, DragEventArgs args)
        {
            if (items.Count == 0) throw new ArgumentException("List must contain elements");
            if (items.Count == 1) return 0;

            Point? previousPoint = null;
            Point? firstPoint = null;
            Point? lastPoint = null;

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var relativePoint = args.GetPosition(item);

                if (item is FrameworkElement element)
                {
                    var shiftedPoint = new Point(relativePoint.X + (element.ActualWidth / 2.0), relativePoint.Y - (element.ActualHeight / 2.0));
                    relativePoint = shiftedPoint;
                }

                if (i == 0)
                {
                    firstPoint = relativePoint;
                }
                else if (i >= items.Count - 1)
                {
                    lastPoint = relativePoint;
                }

                // Check for changes in polarity relative to the last index
                if (previousPoint.HasValue)
                {
                    if (previousPoint.Value.X.IsPolarityChange(relativePoint.X) || previousPoint.Value.Y.IsPolarityChange(relativePoint.Y))
                    {
                        return i;
                    }
                }

                previousPoint = relativePoint;
            }

            // If we never encountered a polarity change, then check the first and last elements
            if (firstPoint.Value.ToVector2().LengthSquared <= lastPoint.Value.ToVector2().LengthSquared)
            {
                return 0;
            }
            else
            {
                return items.Count;
            }
        }
    }
}
