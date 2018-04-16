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

namespace SauceEditor.Controls.ProjectTree
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
