using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities;

namespace SauceEditor.Controls.Properties
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
