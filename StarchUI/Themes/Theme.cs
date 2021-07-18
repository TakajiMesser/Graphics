using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace StarchUICore.Themes
{
    public class Theme
    {
        public Color4 PrimaryForegroundColor { get; set; }
        public Color4 SecondaryForegroundColor { get; set; }
        public Color4 TertiaryForegroundColor { get; set; }

        public Color4 PrimaryBackgroundColor { get; set; }
        public Color4 SecondaryBackgroundColor { get; set; }
        public Color4 TertiaryBackgroundColor { get; set; }

        public Color4 PrimaryTextColor { get; set; }
        public Color4 SecondaryTextColor { get; set; }
        public Color4 TertiaryTextColor { get; set; }
    }
}
