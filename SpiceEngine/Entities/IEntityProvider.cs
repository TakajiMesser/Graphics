using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Lights;
using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public interface IEntityProvider
    {
        IEnumerable<int> EntityRenderIDs { get; }
        IEnumerable<int> EntityScriptIDs { get; }
        IEnumerable<int> EntityPhysicsIDs { get; }

        IEntity GetEntity(int id);
        Actor GetActor(string name);
        EntityTypes GetEntityType(int id);
        List<ILight> Lights { get; }
        List<Actor> Actors { get; }
    }
}
