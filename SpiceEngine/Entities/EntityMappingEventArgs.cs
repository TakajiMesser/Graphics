using System;

namespace SpiceEngine.Entities
{
    public class EntityMappingEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; }

        public EntityMappingEventArgs(IEnumerable<int> ids) => IDs = ids;
    }
}
