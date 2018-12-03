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
    public abstract class MapEntity2D<T> where T : IEntity
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Rotation { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;

        public abstract T ToEntity();
        public abstract Shape2D ToShape();
    }
}
