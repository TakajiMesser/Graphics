using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Maps
{
    // TODO - Determine class design here
    // I might be linking two different concepts in an unfavorable way
    // On one hand, I want a way to tie multiple entities together, so that they can be moved around together
    // However, I also want a way to have a defined archetype from which you can spawn multiple entities off of
    public class MapArchetype
    {
        public string Name { get; set; }

        public List<IEntityBuilder> Entities { get; set; } = new List<IEntityBuilder>();

        //public IEnumerable<Mesh<Vertex3D>> ToMeshes() => throw new NotImplementedException();

        public IEnumerable<IEntity> ToEntities() => throw new NotImplementedException();

        //public IEnumerable<IShape> ToShapes() => throw new NotImplementedException();
    }
}
