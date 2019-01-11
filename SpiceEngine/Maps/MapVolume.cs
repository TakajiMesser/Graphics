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
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Rendering.Meshes;

namespace SpiceEngine.Maps
{
    public class MapVolume : MapEntity3D<Volume>
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<int> TriangleIndices { get; set; } = new List<int>();

        /*public Mesh3D<Vertex3D> ToMesh()
        {
            var vertices = Vertices.Select(v => new Vertex3D(v, v, v, Vector2.Zero, Color4.Blue)).ToList();
            return new Mesh3D<Vertex3D>(vertices, TriangleIndices);
        }*/

        public override Volume ToEntity() => new Volume()
        {
            Position = Position,
            OriginalRotation = Rotation,
            Scale = Scale
        };

        public override Shape3D ToShape() => new Box(Vertices);
    }
}
