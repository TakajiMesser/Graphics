using StarchUICore.Views;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
