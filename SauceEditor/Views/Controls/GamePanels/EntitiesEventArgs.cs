using SauceEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditor.Views.Controls.GamePanels
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
