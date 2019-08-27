using System;
using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public class IDEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; }

        public IDEventArgs(IEnumerable<int> ids) => IDs = ids;
    }
}
