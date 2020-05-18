﻿using SpiceEngineCore.Rendering.Vertices;
using StarchUICore.Attributes.Positions;
using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;
using StarchUICore.Builders;
using StarchUICore.Themes;
using System;

namespace StarchUICore.Views.Controls.Buttons
{
    public class Button : Control
    {
        public event EventHandler OnClicked;

        public override IView Duplicate() => throw new System.NotImplementedException();

        /*protected override MeasuredSize OnMeasure(MeasuredSize availableSize)
        {
            throw new System.NotImplementedException();
        }

        protected override LocatedPosition OnLocate(LocatedPosition availablePosition)
        {
            throw new System.NotImplementedException();
        }*/

        /*public static Button CreateButton(IUnits x, IUnits y, IUnits width, IUnits height)
        {
            if (width is PixelUnits pixelWidth && height is PixelUnits pixelHeight)
            {
                var vertexSet = UIBuilder.Rectangle(pixelWidth.Value, pixelHeight.Value, ThemeManager.CurrentTheme.PrimaryBackgroundColor);
                return new Button(vertexSet)
                {
                    Position = Position.FromOffsets(x, y),
                    Size = Size.FromDimensions(width, height)
                };
            }

            return null;   
        }*/
    }
}