using System;
using System.Reflection;

namespace SauceEditor.Utilities
{
    public static class ReflectionExtensions
    {
        public static bool HasCustomAttribute<T>(this PropertyInfo propertyInfo, bool inherit = true) where T : Attribute
        {
            return propertyInfo.GetCustomAttribute<T>(inherit) != null;
        }

        /*public static bool TryGetCustomAttribute(PropertyInfo propertyInfo, )
        {
            propertyInfo.GetCustomAttribute()

            foreach (var attribute in propertyInfo.GetCustomAttributes(true))
            {
                if (attribute is HideIfNullPropertyAttribute)
                {
                    var propertyValue = propertyInfo.GetValue(this);

                    var visibility = propertyValue != null
                        ? System.Windows.Visibility.Visible
                        : System.Windows.Visibility.Collapsed;

                    propertyDisplayer.SetPropertyVisibility(propertyInfo.Name, visibility);
                }
                else if (attribute is PropertyConversionAttribute propertyConversionAttribute)
                {
                    var propertyValue = propertyInfo.GetValue(this);

                    if (_model != null)
                    {

                    }

                    propertyConversionAttribute;
                }
            }
        }*/
    }
}
