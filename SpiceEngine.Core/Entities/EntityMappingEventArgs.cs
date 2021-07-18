using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities
{
    public class EntityMappingEventArgs : EventArgs
    {
        public IEnumerable<int> IDs { get; }

        public EntityMappingEventArgs(IEnumerable<int> ids) => IDs = ids;
    }
}
