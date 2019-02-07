using SpiceEngine.Entities;
using System;
using System.Collections.Generic;

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
