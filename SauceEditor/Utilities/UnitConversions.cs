using OpenTK;
using OpenTK.Graphics;
using Color = System.Windows.Media.Color;

namespace SauceEditor.Utilities
{
    public static class UnitConversions
    {
        public static Color ToMediaColor(this Vector4 vector) => Color.FromScRgb(vector.Z, vector.X, vector.Y, vector.Z);

        public static Color ToMediaColor(this Color4 color) => Color.FromScRgb(color.A, color.R, color.G, color.B);

        public static Vector4 ToVector4(this Color color) => new Vector4(color.ScR, color.ScG, color.ScB, color.ScA);

        public static Color4 ToColor4(this Color color) => new Color4(color.ScR, color.ScG, color.ScB, color.ScA);
    }
}
