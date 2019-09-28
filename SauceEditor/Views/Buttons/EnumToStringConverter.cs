using System;
using System.Globalization;
using System.Windows.Data;

namespace SauceEditor.Views.Buttons
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            //value != null ? Enum.GetName(value.GetType(), value) : string.Empty;
            value != null ? Enum.GetName(value.GetType(), value) : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value != null ? Enum.Parse(targetType, value.ToString()) : null;
    }
}
