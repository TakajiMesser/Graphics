using SauceEditorCore.Models.Components;
using System.Collections.Generic;

namespace SauceEditorCore.Models.Libraries
{
    public interface ILibrary
    {
        string Name { get; }
        string Path { get; set; }
        IEnumerable<IComponent> Components { get; }

        void Load();
    }
}
