using System;
using System.Windows;

namespace SauceEditor.Views.ProjectTree
{
    /// <summary>
    /// Interaction logic for FileNameDialog.xaml
    /// </summary>
    public partial class FileNameDialog : Window
    {
        public string FileName => txtName.Text;

        public FileNameDialog() => InitializeComponent();
        public FileNameDialog(string initialName)
        {
            InitializeComponent();

            txtName.Text = initialName;
            txtName.SelectAll();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtName.SelectAll();
            txtName.Focus();
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}
