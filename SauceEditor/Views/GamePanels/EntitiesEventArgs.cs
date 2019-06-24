using SauceEditor.Models;
using SauceEditorCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditor.Views.GamePanels
{
    public class EntitiesEventArgs : EventArgs
    {
        public List<EditorEntity> Entities { get; private set; }

        public EntitiesEventArgs(IEnumerable<EditorEntity> entities)
        {
            Entities = entities.ToList();
        }
    }
}
