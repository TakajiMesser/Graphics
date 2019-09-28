using System;
using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public class EntityMappingEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; }

        public EntityMappingEventArgs(IEnumerable<int> ids) => IDs = ids;
    }
}
