using SauceEditor.Models;
using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Maps;
using System;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Scripts
{
    /// <summary>
    /// Interaction logic for ScriptView.xaml
    /// </summary>
    public partial class ScriptView : LayoutAnchorable, IHaveDockViewModel, IFile
    {
        public EventHandler<ScriptEventArgs> Saved;

        public ScriptView()
        {
            InitializeComponent();
            ViewModel.Filer = this;
        }

        public DockViewModel GetViewModel() => ViewModel;

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
