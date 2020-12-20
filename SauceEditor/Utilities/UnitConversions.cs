using System.Windows;
using Color = System.Windows.Media.Color;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SauceEditor.Utilities
{
    public static class UnitConversions
    {
        public static Color ToMediaColor(this Vector4 vector) => Color.FromScRgb(vector.Z, vector.X, vector.Y, vector.Z);

        public static Color ToMediaColor(this Color4 color) => Color.FromScRgb(color.A, color.R, color.G, color.B);

        public static Vector4 ToVector4(this Color color) => new Vector4(color.ScR, color.ScG, color.ScB, color.ScA);

        public static Color4 ToColor4(this Color color) => new Color4(color.ScR, color.ScG, color.ScB, color.ScA);

        public static Vector2 ToVector2(this Vector vector) => new Vector2((float)vector.X, (float)vector.Y);

        public static Vector2 ToVector2(this Point point) => new Vector2((float)point.X, (float)point.Y);

        public static System.Windows.Point ToWindowsPoint(this System.Drawing.Point point) => new Point(point.X, point.Y);

        public static System.Drawing.Point ToDrawingPoint(this System.Windows.Point point) => new System.Drawing.Point((int)point.X, (int)point.Y);
    }
}
