using System;

namespace SpiceEngine.Entities
{
    public class IDEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; }

        public EntityEventArgs(IEnumerable<int> ids) => IDs = ids;
    }
}
