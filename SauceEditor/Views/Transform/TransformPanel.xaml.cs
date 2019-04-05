using OpenTK;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SauceEditor.Views.Transform
{
    /// <summary>
    /// Interaction logic for TransformPanel.xaml
    /// </summary>
    public partial class TransformPanel : UserControl
    {
        public readonly static DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(TransformPanel));
        public readonly static DependencyProperty TransformProperty = DependencyProperty.Register("Transform", typeof(Vector3), typeof(TransformPanel));

        public event EventHandler<TransformChangedEventArgs> TransformChanged;

        public TransformPanel()
        {
            DataContext = this;
            InitializeComponent();

            X_UpDown.ValueChanged += (s, args) => TransformChanged?.Invoke(this, new TransformChangedEventArgs(Label, new Vector3(args.NewValue, Y_UpDown.Value, Z_UpDown.Value)));
            Y_UpDown.ValueChanged += (s, args) => TransformChanged?.Invoke(this, new TransformChangedEventArgs(Label, new Vector3(X_UpDown.Value, args.NewValue, Z_UpDown.Value)));
            Z_UpDown.ValueChanged += (s, args) => TransformChanged?.Invoke(this, new TransformChangedEventArgs(Label, new Vector3(X_UpDown.Value, Y_UpDown.Value, args.NewValue)));
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set
            {
                Title.Visibility = !string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
                SetValue(LabelProperty, value);
            }
        }

        public Vector3 Transform
        {
            get => (Vector3)GetValue(TransformProperty);
            set
            {
                SetValue(TransformProperty, value);
            }
        }
    }
}
