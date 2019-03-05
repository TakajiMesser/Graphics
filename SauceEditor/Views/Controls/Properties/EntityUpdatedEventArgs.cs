using SpiceEngine.Entities;
using System;

namespace SauceEditor.Views.Controls.Properties
{
    public class EntityUpdatedEventArgs : EventArgs
    {
        public IEntity Entity { get; private set; }

        public EntityUpdatedEventArgs(IEntity entity)
        {
            Entity = entity;
        }
    }
}
