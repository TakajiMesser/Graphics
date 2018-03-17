using System;
using TakoEngine.Entities;

namespace TakoEngine.Game
{
    public class EntitySelectedEventArgs : EventArgs
    {
        public IEntity Entity { get; private set; }

        public EntitySelectedEventArgs(IEntity entity)
        {
            Entity = entity;
        }
    }
}
