using System;
using System.Collections.Generic;
using SpiceEngine.Entities;

namespace SpiceEngine.Game
{
    public class DuplicatedEntityEventArgs : EventArgs
    {
        public int ID { get; private set; }
        public int NewID { get; private set; }

        public DuplicatedEntityEventArgs(int id, int newID)
        {
            ID = id;
            NewID = newID;
        }
    }
}
