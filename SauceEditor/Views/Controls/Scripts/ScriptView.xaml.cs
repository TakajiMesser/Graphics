using System;
using System.Collections.Generic;
using System.IO;
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

namespace SauceEditor.Views.Controls.Scripts
{
    /// <summary>
    /// Interaction logic for ScriptView.xaml
    /// </summary>
    public partial class ScriptView : DockPanel
    {
        public ScriptView(string scriptPath)
        {
            InitializeComponent();
            Open(scriptPath);
        }

        public void Open(string filePath)
        {
            LockEditor();
            //TextEditor.Text = File.ReadAllText(filePath);
            TextEditor.Load(filePath);
            UnlockEditor();
        }

        public void Save(string filePath)
        {
            LockEditor();
            TextEditor.Save(filePath);
            UnlockEditor();
        }

        public void LockEditor()
        {
            TextEditor.IsManipulationEnabled = false;
        }

        public void UnlockEditor()
        {
            TextEditor.IsManipulationEnabled = true;
        }
    }
}
