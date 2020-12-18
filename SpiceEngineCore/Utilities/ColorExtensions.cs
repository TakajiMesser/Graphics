using OpenTK;
using OpenTK.Graphics;

namespace SpiceEngineCore.Utilities
{
    public static class ColorExtensions
    {
        public static Vector3 ToRGB(this Color4 color) => new Vector3(color.R, color.G, color.B);

        public static Vector4 ToVector4(this Color4 color) => new Vector4(color.R, color.G, color.B, color.A);
    }
}
