using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Lights;
using System.Collections.Generic;

namespace SpiceEngine.Entities
{
    public interface IEntityProvider
    {
        IEntity GetEntity(int id);
        EntityTypes GetEntityType(int id);
        List<Light> Lights { get; }
        List<Actor> Actors { get; }
    }
}
