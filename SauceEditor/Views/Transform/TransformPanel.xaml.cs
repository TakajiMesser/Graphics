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

        public TransformPanel()
        {
            InitializeComponent();

            ViewModel.XViewModel = X_UpDown.ViewModel;
            ViewModel.YViewModel = Y_UpDown.ViewModel;
            ViewModel.ZViewModel = Z_UpDown.ViewModel;
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
    }
}
