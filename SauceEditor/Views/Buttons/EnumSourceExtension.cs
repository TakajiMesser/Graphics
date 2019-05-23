using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SauceEditor.Views.Buttons
{
    public class EnumSourceExtension : MarkupExtension
    {
        private Type _enumType;
        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (value != null)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum) throw new ArgumentException("Type must be for an Enum.");
                    }

                    _enumType = value;
                }
            }
        }

        private Binding _binding = null;

        public EnumSourceExtension() { }
        public EnumSourceExtension(Type enumType) => EnumType = enumType;
        public EnumSourceExtension(Enum enumValue) => EnumType = enumValue.GetType();
        public EnumSourceExtension(Binding binding) => _binding = binding;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_binding != null)
            {
                _enumType = _binding.ProvideValue(serviceProvider).GetType();
            }

            if (_enumType == null) throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
            {
                return enumValues;
            }

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
