using OpenTK;
using OpenTK.Graphics;
using System;

namespace SpiceEngine.Utilities
{
    public static class ColorExtensions
    {
        public static Vector3 ToRGB(this Color4 color) => new Vector3(color.R, color.G, color.B);
    }
}
