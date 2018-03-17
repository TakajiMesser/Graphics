using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SauceEditor.Controls
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

        public NumericUpDown()
        {
            DataContext = this;
            InitializeComponent();
            Value = DefaultValue;
        }

        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set
            {
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(Value, value));
                SetValue(ValueProperty, value);
            }
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

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }
    }
}
