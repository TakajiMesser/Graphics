using SauceEditor.ViewModels.Docks;
using SauceEditorCore.Models.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LibraryManager = SauceEditorCore.Models.Libraries.LibraryManager;

namespace SauceEditor.ViewModels.Libraries
{
    public class LibraryPanelViewModel : DockViewModel
    {
        //private LibraryManager _libraryManager = new LibraryManager();
        private List<IComponent> _components = new List<IComponent>();

        public ReadOnlyCollection<IComponent> Components { get; set; }

        public LibraryPanelViewModel() : base(DockTypes.Property) => Components = new ReadOnlyCollection<IComponent>(_components);

        public void UpdateFromModel(LibraryManager libraryManager)
        {
            //libraryManager.
            libraryManager.Load();
            _components.Add(libraryManager.MapLibrary.GetComponentAt(0));
        }
    }
}