using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SauceEditor.Controls.Transform
{
    /// <summary>
    /// Interaction logic for TransformPanel.xaml
    /// </summary>
    public partial class TransformPanel : UserControl
    {
        public readonly static DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(TransformPanel));

        public event EventHandler<TransformChangedEventArgs> TransformChanged;

        public TransformPanel()
        {
            DataContext = this;
            InitializeComponent();

            X.ValueChanged += (s, args) => TransformChanged?.Invoke(this, new TransformChangedEventArgs(Label, new Vector3(args.NewValue, Y.Value, Z.Value)));
            Y.ValueChanged += (s, args) => TransformChanged?.Invoke(this, new TransformChangedEventArgs(Label, new Vector3(X.Value, args.NewValue, Z.Value)));
            Z.ValueChanged += (s, args) => TransformChanged?.Invoke(this, new TransformChangedEventArgs(Label, new Vector3(X.Value, Y.Value, args.NewValue)));
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

        public void SetValues(Vector3 values)
        {
            X.Value = values.X;
            Y.Value = values.Y;
            Z.Value = values.Z;
        }
    }
}
