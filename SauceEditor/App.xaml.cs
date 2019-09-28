using System;
using System.Windows;

namespace SauceEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public enum Themes
        {
            Light,
            Dark
        }

        public App()
        {
            InitializeComponent();
            ChangeTheme(Themes.Dark);
        }

        public void ChangeTheme(Themes theme)
        {
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = GetSourceUri(theme)
            });
        }

        private Uri GetSourceUri(Themes theme)
        {
            switch (theme)
            {
                case Themes.Light:
                    return new Uri("Themes/Light.xaml", UriKind.Relative);
                case Themes.Dark:
                    return new Uri("Themes/Dark.xaml", UriKind.Relative);
            }

            throw new NotImplementedException();
        }
    }
}
