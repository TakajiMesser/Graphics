using System;

namespace SpiceEngineCore.Maps
{
    public class EntityMapEventArgs : EventArgs
    {
        public EntityMap EntityMap { get; }

        public EntityMapEventArgs(EntityMap entityMap) => EntityMap = entityMap;
    }
}
