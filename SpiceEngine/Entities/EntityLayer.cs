using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public class EntityLayer
    {
        private List<int> _entityIDs = new List<int>();

        public string Name { get; private set; }
        public IEnumerable<int> EntityIDs => _entityIDs;

        public int Count => _entityIDs.Count;

        public EntityLayer(string name)
        {
            Name = name;
        }

        public bool Contains(int entityID) => _entityIDs.Contains(entityID);
    }
}
