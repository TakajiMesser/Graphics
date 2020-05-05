using OpenTK;
using OpenTK.Graphics;
using StarchUICore.Views;
using System;

namespace StarchUICore
{
    public class LayoutEventArgs : EventArgs
    {
        public LayoutEventArgs(IElement element)
        {
            Element = element;

            Position = new Vector3(element.Measurement.X, element.Measurement.Y, 0.0f);
            Size = new Vector2(element.Measurement.Width, element.Measurement.Height);
            
            if (element is IView view)
            {
                Color = new Color4(view.Background.Color.R, view.Background.Color.G, view.Background.Color.B, element.Alpha);
                BorderThickness = element.Border.Thickness;
                BorderColor = element.Border.Color;
                CornerRadius = new Vector2(element.Border.CornerXRadius, element.Border.CornerYRadius);
            }
            else
            {
                Color = Color4.Transparent;
                BorderThickness = 2.0f;
                BorderColor = Color4.White;
                CornerRadius = Vector2.Zero;
            }
        }

        public IElement Element { get; }
        
        public Vector3 Position { get; }
        public Vector2 Size { get; }
        public Color4 Color { get; }

        public float BorderThickness { get; }
        public Color4 BorderColor { get; }
        public Vector2 CornerRadius { get; }
    }
}
