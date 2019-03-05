using OpenTK;
using OpenTK.Graphics;
using System.Drawing;

namespace SpiceEngine.Utilities
{
    public static class OpenTKExtensions
    {
        public static Vector2 ToVector2(this Point point) => new Vector2(point.X, point.Y);

        public static Vector4 ToVector4(this Color4 color) => new Vector4(color.R, color.G, color.B, color.A);

        public static Color4 ToColor4(this Vector4 vector) => new Color4(vector.X, vector.Y, vector.Z, vector.W);
    }
}
