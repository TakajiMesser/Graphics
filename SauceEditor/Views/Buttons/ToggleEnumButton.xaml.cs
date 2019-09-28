using System;
using System.Windows;
using System.Windows.Controls;

namespace SauceEditor.Views.Buttons
{
    /// <summary>
    /// Interaction logic for ToggleEnumButton.xaml
    /// </summary>
    public partial class ToggleEnumButton : UserControl
    {
        public readonly static DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ToggleEnumButton),
            new PropertyMetadata(Orientation.Horizontal));

        public readonly static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Enum), typeof(ToggleEnumButton),
            new PropertyMetadata(ValueChanged_Callback));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public Enum Value
        {
            get => (Enum)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
                UpdateItemsSource(value.GetType());
            }
        }

        public ToggleEnumButton()
        {
            InitializeComponent();
        }

        private static void ValueChanged_Callback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is ToggleEnumButton owner)
            {
                owner.UpdateItemsSource(args.NewValue.GetType());
            }
        }

        private void UpdateItemsSource(Type enumType)
        {
            var actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            var enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == enumType)
            {
                if (MainList.ItemsSource != enumValues)
                {
                    MainList.ItemsSource = enumValues;
                }
            }
            else
            {
                var values = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
                enumValues.CopyTo(values, 1);
                MainList.ItemsSource = values;
            }
        }
    }
}
