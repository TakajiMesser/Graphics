using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Actors;

namespace SpiceEngine.Entities
{
    public interface IEntityProvider
    {
        IEnumerable<int> EntityRenderIDs { get; }
        IEnumerable<int> EntityScriptIDs { get; }
        IEnumerable<int> EntityPhysicsIDs { get; }

        IEntity GetEntity(int id);
        List<Light> Lights { get; }
        List<Actor> Actors { get; }
    }
}
