using OpenTK.Graphics;

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
