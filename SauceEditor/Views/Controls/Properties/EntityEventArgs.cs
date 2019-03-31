using SauceEditor.Models;
using System;

namespace SauceEditor.Views.Controls.Properties
{
    public class EntityEventArgs : EventArgs
    {
        public EditorEntity Entity { get; private set; }

        public EntityEventArgs(EditorEntity entity)
        {
            Entity = entity;
        }
    }
}
