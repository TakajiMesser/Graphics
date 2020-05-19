using System.Windows;
using System.Windows.Media;

namespace SauceEditor.Helpers
{
    public static class ViewHelper
    {
        public static T FindAncestor<T>(DependencyObject source) where T : DependencyObject
        {
            var current = source;

            do
            {
                if (current is T t)
                {
                    return t;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);

            return null;
        }
    }
}
