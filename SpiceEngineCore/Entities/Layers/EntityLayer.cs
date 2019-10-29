using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Entities.Layers
{
    public class EntityLayer
    {
        private HashSet<int> _entityIDs = new HashSet<int>();

        public string Name { get; private set; }
        public IEnumerable<int> EntityIDs => _entityIDs;

        public int Count => _entityIDs.Count;

        public EntityLayer(string name) => Name = name;

        public void Add(int entityID) => _entityIDs.Add(entityID);
        public void Remove(int entityID) => _entityIDs.Remove(entityID);
        public bool Contains(int entityID) => _entityIDs.Contains(entityID);
        public void Clear() => _entityIDs.Clear();
    }
}
