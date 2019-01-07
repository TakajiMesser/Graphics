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
        public Vector4 Color { get; set; }

        /*public Mesh3D<Vertex3D> ToMesh()
        {
            var vertices = Vertices.Select(v => new Vertex3D(v, v, v, Vector2.Zero, Color4.Blue)).ToList();
            return new Mesh3D<Vertex3D>(vertices, TriangleIndices);
        }*/

        public override Volume ToEntity() => new Volume(Vertices, TriangleIndices, Color)
        {
            Position = Position,
            OriginalRotation = Rotation,
            Scale = Scale
        };

        public override Shape3D ToShape() => new Box(Vertices);

        public static MapBrush Rectangle(Vector3 center, float width, float height)
        {
            return new MapBrush()
            {
                Vertices = new List<Vertex3D>
                {
                    new Vertex3D(new Vector3(-width / 2.0f, -height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)),
                    new Vertex3D(new Vector3(-width / 2.0f, height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)),
                    new Vertex3D(new Vector3(width / 2.0f, -height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)),
                    new Vertex3D(new Vector3(width / 2.0f, height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1))
                },
                Position = center,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2,
                TriangleIndices = new List<int>()
                {
                    0, 2, 1, 1, 2, 3
                }
            };
        }

        public static MapVolume RectangularPrism(Vector3 center, float width, float height, float depth)
        {
            var vertices = new List<Vector3>
            {
                new Vector3(-width / 2.0f, -height / 2.0f, -depth / 2.0f),
                new Vector3(-width / 2.0f, height / 2.0f, -depth / 2.0f),
                new Vector3(-width / 2.0f, -height / 2.0f, depth / 2.0f),
                new Vector3(-width / 2.0f, height / 2.0f, depth / 2.0f),
                new Vector3(width / 2.0f, -height / 2.0f, -depth / 2.0f),
                new Vector3(width / 2.0f, height / 2.0f, -depth / 2.0f),
                new Vector3(width / 2.0f, -height / 2.0f, depth / 2.0f),
                new Vector3(width / 2.0f, height / 2.0f, depth / 2.0f)
            };

            var vertexIndices = new List<int>
            {
                8, 7, 5, 8, 5, 6,
                2, 4, 8, 2, 8, 6,
                4, 3, 7, 4, 7, 8,
                2, 1, 3, 2, 3, 4, 
                3, 1, 5, 3, 5, 7,
                6, 5, 1, 6, 1, 2
            };

            var verticies = new List<Vector3>();
            var triangleIndices = new List<int>();

            for (var i = 0; i < vertexIndices.Count; i++)
            {
                // Grab vertexIndices, three at a time, to form each triangle
                if (i % 3 == 0)
                {
                    // For a given triangle with vertex positions P0, P1, P2 and corresponding UV texture coordinates T0, T1, and T2:
                    // deltaPos1 = P1 - P0;
                    // delgaPos2 = P2 - P0;
                    // deltaUv1 = T1 - T0;
                    // deltaUv2 = T2 - T0;
                    // r = 1 / (deltaUv1.x * deltaUv2.y - deltaUv1.y - deltaUv2.x);
                    // tangent = (deltaPos1 * deltaUv2.y - deltaPos2 * deltaUv1.y) * r;
                    var deltaPos1 = vertices[vertexIndices[i + 1] - 1] - vertices[vertexIndices[i] - 1];
                    var deltaPos2 = vertices[vertexIndices[i + 2] - 1] - vertices[vertexIndices[i] - 1];
                }

                var meshVertex = vertices[vertexIndices[i] - 1];
                var existingIndex = verticies.FindIndex(v => v == meshVertex);

                if (existingIndex >= 0)
                {
                    triangleIndices.Add(existingIndex);
                }
                else
                {
                    triangleIndices.Add(verticies.Count);
                    verticies.Add(meshVertex);
                }
            }

            return new MapVolume()
            {
                Position = center,
                Vertices = verticies,
                TriangleIndices = triangleIndices
            };

            /*return new MapBrush()
            {
                Vertices = new List<Vertex>
                {
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1)),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1))
                },
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2,
                TriangleIndices = new List<int>()
                {
                    6, 7, 4, 7, 4, 5, 3, 1, 7, 1, 7, 5, 2, 3, 6, 3, 6, 7, 0, 1, 2, 1, 2, 3, 0, 2, 4, 2, 4, 6, 4, 5, 0, 5, 0, 1
                }
            };*/
        }
    }
}
