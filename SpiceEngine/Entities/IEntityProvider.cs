using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Actors;

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
