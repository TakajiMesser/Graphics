using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Entities;

namespace SauceEditor.Controls
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
