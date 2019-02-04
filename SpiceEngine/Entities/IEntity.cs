using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Scripting.StimResponse;

namespace SpiceEngine.Entities
{
    public interface IEntity
    {
        int ID { get; set; }
        Vector3 Position { get; set; }
        //IEntity Duplicate();
    }
}
