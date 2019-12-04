using OpenTK.Graphics;
using System;

namespace SpiceEngineCore.Rendering
{
    public class ColorEventArgs : EventArgs
    {
        public Color4 Color { get; }

        public ColorEventArgs(Color4 color) => Color = color;
    }
}
