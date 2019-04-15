using OpenTK;

namespace SpiceEngine.Entities
{
    /// <summary>
    /// An Archetype should define an Entity or a collection of Entities that can be used to spawn multiple Entities
    /// An Archetype should track all Entities that were spawned off of it to allow scripts to trigger on all of them
    /// </summary>
    public class Archetype
    {
        public string Name { get; private set; }

        public List<int> SpawnedEntityIDs { get; private set; } = new List<int>();

        public Archetype(string name)
        {
            Name = name;
        }

        public IEnumerable<IEntity> SpawnEntities()
        {
            throw new NotImplementedException();
        }
    }
}
