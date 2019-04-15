using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Maps
{
    // TODO - Determine class design here
    // I might be linking two different concepts in an unfavorable way
    // On one hand, I want a way to tie multiple entities together, so that they can be moved around together
    // However, I also want a way to have a defined archetype from which you can spawn multiple entities off of
    public class MapArchetype
    {
        public string Name { get; set; }

        public List<IMapEntity3D> Entities { get; set; } = new List<IMapEntity3D>();

        public IEnumerable<Mesh3D<Vertex3D>> ToMeshes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEntity> ToEntities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shape3D> ToShapes()
        {
            throw new NotImplementedException();
        }
    }
}
