using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngine.Entities.Lights;

namespace SpiceEngine.Entities
{
    public interface IEntityProvider
    {
        IEntity GetEntity(int id);
        List<Light> Lights { get; }
    }
}
