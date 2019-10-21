using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;

namespace SauceEditor.ViewModels.Scripts
{
    public class ScriptPanelViewModel : DockableViewModel
    {
        public IFile Filer { get; set; }
        public ScriptComponent Script { get; set; }

        public void UpdateFromModel(ScriptComponent script)
        {
            Script = script;
            Filer?.Load(script.Path);
        }
    }
}