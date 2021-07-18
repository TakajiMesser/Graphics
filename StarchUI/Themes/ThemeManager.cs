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
    public class ThemeManager
    {
        private static ThemeManager _instance;
        public static ThemeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ThemeManager();
                }

                return _instance;
            }
        }

        private Theme _defaultTheme;

        private ThemeManager()
        {
            _defaultTheme = new Theme()
            {
                PrimaryForegroundColor = Color4.Black,
                SecondaryForegroundColor = Color4.DarkGray,
                TertiaryForegroundColor = Color4.LightGray,
                PrimaryBackgroundColor = Color4.White,
                SecondaryBackgroundColor = Color4.WhiteSmoke,
                TertiaryBackgroundColor = Color4.NavajoWhite
            };
        }

        public static Theme CurrentTheme => Instance._defaultTheme;
    }
}
