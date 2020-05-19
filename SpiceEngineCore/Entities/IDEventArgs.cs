using SpiceEngineCore.Utilities;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public class IDEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; }

        public IDEventArgs(int id) => IDs = id.Yield();
        public IDEventArgs(IEnumerable<int> ids) => IDs = ids;
    }
}
