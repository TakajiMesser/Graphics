using SauceEditor.Models;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Maps;
using System;
using System.Windows.Controls;

namespace SauceEditor.Views.Controls.Scripts
{
    /// <summary>
    /// Interaction logic for ScriptView.xaml
    /// </summary>
    public partial class ScriptView : DockPanel
    {
        private Script _script;

        public EventHandler<ScriptEventArgs> Saved;

        public ScriptView(string scriptPath)
        {
            InitializeComponent();

            _script = new Script();
            Open(scriptPath);
        }

        public ScriptView(string scriptPath, Actor actor, MapActor mapActor)
        {
            InitializeComponent();

            _script = new Script();
            _script.SetEntities(actor, mapActor);

            Open(scriptPath);
        }

        public void Open(string filePath)
        {
            LockEditor();
            //TextEditor.Text = File.ReadAllText(filePath);
            _script.Load(filePath);
            TextEditor.Load(filePath);
            UnlockEditor();
        }

        public void Save(string filePath)
        {
            LockEditor();

            TextEditor.Save(filePath);
            Saved?.Invoke(this, new ScriptEventArgs(_script));

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
