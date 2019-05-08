using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;
using SpiceEngine.Maps;

namespace SauceEditor.ViewModels
{
    public class ScriptViewModel : MainDockViewModel
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