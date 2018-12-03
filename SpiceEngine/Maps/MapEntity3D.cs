using OpenTK;
using System.Collections.Generic;
using System.Linq;
using SpiceEngine.Entities;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using OpenTK.Graphics;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Maps
{
    public abstract class MapEntity3D<T> where T : IEntity
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public abstract T ToEntity();
        public abstract Shape3D ToShape();
    }
}
