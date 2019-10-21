using SauceEditor.Views.Factories;
using System;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Scripts
{
    /// <summary>
    /// Interaction logic for ScriptPanel.xaml
    /// </summary>
    public partial class ScriptPanel : LayoutAnchorable, IFile
    {
        public EventHandler<ScriptEventArgs> Saved;

        public ScriptPanel()
        {
            InitializeComponent();
            ViewModel.Filer = this;
        }

        public void Load(string filePath)
        {
            LockEditor();
            TextEditor.Load(filePath);
            UnlockEditor();
        }

        public void Save(string filePath)
        {
            LockEditor();
            TextEditor.Save(filePath);
            Saved?.Invoke(this, new ScriptEventArgs(ViewModel.Script));
            UnlockEditor();
        }

        public void LockEditor() => TextEditor.IsManipulationEnabled = false;

        public void UnlockEditor() => TextEditor.IsManipulationEnabled = true;
    }
}
