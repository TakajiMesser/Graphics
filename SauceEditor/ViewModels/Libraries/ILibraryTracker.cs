using SauceEditorCore.Models.Components;
using System.Collections.Generic;

namespace SauceEditor.ViewModels.Libraries
{
    // TODO - Rename this to something less shit
    public interface ILibraryTracker
    {
        void OpenLibrary(string name, IEnumerable<PathInfoViewModel> items);
        void OpenComponent(ComponentInfo componentInfo);
    }
}