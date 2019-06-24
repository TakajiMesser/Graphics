using SauceEditor.Models;
using SauceEditorCore.Models.Entities;
using System;

namespace SauceEditor.Views.Properties
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
