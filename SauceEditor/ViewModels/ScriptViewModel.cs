using SauceEditor.Models.Components;
using SauceEditor.Views.Factories;

namespace SauceEditor.ViewModels
{
    public class ScriptViewModel : ViewModel
    {
        public IFile Filer { get; set; }
        public Script Script { get; set; }

        public void UpdateFromModel(Script script)
        {
            Script = script;
            Filer?.Load(script.Path);
        }
    }
}