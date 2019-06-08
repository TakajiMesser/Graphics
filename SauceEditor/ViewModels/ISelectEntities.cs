using SauceEditor.Models;
using SpiceEngine.Maps;
using System.Collections.Generic;

namespace SauceEditor.ViewModels
{
    public interface ISelectEntities
    {
        List<EditorEntity> SelectedEntities { get; set; }
    }
}
