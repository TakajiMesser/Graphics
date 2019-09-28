using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Components
{
    public interface IComponent
    {
        string Path { get; set; }
        string Name { get; }

        //void Load();
        //void Save();
    }
}
