using OpenTK;
using SpiceEngine.Entities;
using System;

namespace SpiceEngine.Entities.Builders
{
    public interface IEntityBuilder
    {
        Vector3 Position { get; set; }

        IEntity ToEntity();
        Type GetEntityType();
    }
}
