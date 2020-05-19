using SpiceEngineCore.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SauceEditor.Views.UpDowns
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public readonly static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(NumericUpDown));
        public readonly static DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(float), typeof(NumericUpDown), new UIPropertyMetadata(0.00f));
        public readonly static DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(float), typeof(NumericUpDown), new UIPropertyMetadata(0.01f));
        public readonly static DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(float), typeof(NumericUpDown));
        public readonly static DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(float), typeof(NumericUpDown));

        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler<ValueChangedEventArgs> ValueHoldChanged;

        private float _downValue;
        private Point _cursorStart = new Point();

        public NumericUpDown()
        {
            InitializeComponent();
            ViewModel.Value = DefaultValue;
        }

        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public float DefaultValue
        {
            get => (float)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public float Step
        {
            get => (float)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public float MaxValue
        {
            get => (float)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public float MinValue
        {
            get => (float)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        private void UpButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _downValue = ViewModel.Value;
            _cursorStart = Mouse.GetPosition(this);
        }

        private void DownButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _downValue = ViewModel.Value;
            _cursorStart = Mouse.GetPosition(this);
        }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ValueHoldChanged?.Invoke(this, new ValueChangedEventArgs(_downValue, ViewModel.Value));
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var point = Mouse.GetPosition(this);

            var step = Step;
            var dy = (float)(_cursorStart.Y - point.Y);
            if (dy > 0)
            {
                step *= dy;
            }

            ViewModel.Value = (ViewModel.Value + step).Clamp(MinValue, MaxValue);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var point = Mouse.GetPosition(this);

            var step = Step;
            var dy = (float)(point.Y - _cursorStart.Y);
            if (dy > 0)
            {
                step *= dy;
            }

            ViewModel.Value = (ViewModel.Value - step).Clamp(MinValue, MaxValue);
        }
    }
}
