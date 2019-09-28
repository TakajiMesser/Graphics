using SauceEditor.Views.Tools;
using SauceEditorCore.Models.Libraries;

namespace SauceEditor.ViewModels.Tools
{
    public class LibraryPanelViewModel : ViewModel
    {
        public LibraryManager LibraryManager { get; set; }

        //public LibraryPage Page { get; set; }

        /*public Library MapLibrary { get; private set; }
        public Library ModelLibrary { get; private set; }
        public Library BehaviorLibrary { get; private set; }
        public Library TextureLibrary { get; private set; }
        public Library SoundLibrary { get; private set; }
        public Library MaterialLibrary { get; private set; }
        public Library ArchetypeLibrary { get; private set; }
        public Library ScriptLibrary { get; private set; }*/

        public void OnLibraryManagerChanged()
        {
            //Page = new LibraryPage(LibraryManager.MapLibrary);
        }
    }
}