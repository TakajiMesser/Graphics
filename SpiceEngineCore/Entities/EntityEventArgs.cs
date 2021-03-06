﻿using System;

namespace SpiceEngineCore.Entities
{
    public class EntityEventArgs : EventArgs
    {
        public int ID { get; }

        public EntityEventArgs(int id) => ID = id;
    }
}
