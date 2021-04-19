using System;

namespace SpiceEngineCore.Maps
{
    public class EntityMappingEventArgs : EventArgs
    {
        public EntityMapping EntityMapping { get; }

        public EntityMappingEventArgs(EntityMapping entityMapping) => EntityMapping = entityMapping;
    }
}
