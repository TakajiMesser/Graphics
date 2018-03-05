using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DockingLibrary
{
    class DockableContentTabItemsPanel : System.Windows.Controls.Panel
    {
        private double _totChildWidth = 0.0;

        protected override Size MeasureOverride(Size availableSize)
        {
            Size childSize = availableSize;
            Size totalDesideredSize = new Size(availableSize.Width, 0.0);
            _totChildWidth = 0.0;

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(childSize);

                _totChildWidth += child.DesiredSize.Width;
                if (totalDesideredSize.Height < child.DesiredSize.Height)
                {
                    totalDesideredSize.Height = child.DesiredSize.Height;
                }
            }

            return totalDesideredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size inflate = new Size();

            if (finalSize.Width < _totChildWidth)
            {
                inflate.Width = (_totChildWidth - finalSize.Width) / InternalChildren.Count;
            }
            
            Point offset = new Point();
            if (finalSize.Width > _totChildWidth)
            {
                offset.X = -(finalSize.Width - _totChildWidth) / 2;
            }

            double totalFinalWidth = 0.0;
            foreach (UIElement child in InternalChildren)
            {
                Size childFinalSize = child.DesiredSize;
                childFinalSize.Width -= inflate.Width;
                childFinalSize.Height = finalSize.Height;

                child.Arrange(new Rect(offset, childFinalSize));

                offset.Offset(childFinalSize.Width, 0);
                totalFinalWidth += childFinalSize.Width;
            }

            return new Size(totalFinalWidth, finalSize.Height);
        }
    }
}
