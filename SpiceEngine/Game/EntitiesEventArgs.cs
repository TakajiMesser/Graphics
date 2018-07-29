using System;
using System.Collections.Generic;
using SpiceEngine.Entities;

namespace SpiceEngine.Game
{
    public class EntitiesEventArgs : EventArgs
    {
        public List<IEntity> Entities { get; private set; }

        public EntitiesEventArgs(IEnumerable<IEntity> entities)
        {
            Entities = new List<IEntity>(entities);
        }
    }
}
