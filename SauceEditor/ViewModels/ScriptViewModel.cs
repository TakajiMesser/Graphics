using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Factories;

namespace SauceEditor.ViewModels
{
    public class ScriptViewModel : DockViewModel
    {
        public ScriptViewModel() : base(DockTypes.Game) { }

        public IFile Filer { get; set; }
        public ScriptComponent Script { get; set; }

        public void UpdateFromModel(ScriptComponent script)
        {
            Script = script;
            Filer?.Load(script.Path);
        }
    }
}