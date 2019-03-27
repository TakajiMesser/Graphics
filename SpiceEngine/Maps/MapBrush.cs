using OpenTK;
using OpenTK.Graphics;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Helpers;
using SpiceEngine.Physics.Shapes;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Maps
{
    public class MapBrush : MapEntity3D<Brush>
    {
        public List<Vertex3D> Vertices { get; set; } = new List<Vertex3D>();
        public Material Material { get; set; }
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public bool IsPhysical { get; set; }
        public TexturePaths TexturesPaths { get; set; } = new TexturePaths();

        public Mesh3D<Vertex3D> ToMesh()
        {
            if (TexturesPaths.IsEmpty)
            {
                AddTestColors();
            }
            
            return new Mesh3D<Vertex3D>(Vertices, TriangleIndices);
        }

        public override Brush ToEntity() => new Brush(Material)
        {
            Position = Position,
            OriginalRotation = Rotation,
            Scale = Scale,
            //HasCollision = HasCollision
        };

        public override Shape3D ToShape() => new Box(Vertices.Select(v => v.Position));

        public void AddTestColors()
        {
            for (var i = 0; i < Vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    Vertices[i] = (Vertex3D)Vertices[i].Colored(Color4.Lime);
                }
                else if (i % 3 == 1)
                {
                    Vertices[i] = (Vertex3D)Vertices[i].Colored(Color4.Red);
                }
                else if (i % 3 == 2)
                {
                    Vertices[i] = (Vertex3D)Vertices[i].Colored(Color4.Blue);
                }
            }
        }

        public static MapBrush Rectangle(Vector3 center, float width, float height)
        {
            var meshShape = MeshShape.Rectangle(width, height);

            var vertices = new List<Vertex3D>()
            {
                new Vertex3D(meshShape.Vertices[0], Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)),
                new Vertex3D(meshShape.Vertices[1], Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)),
                new Vertex3D(meshShape.Vertices[2], Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)),
                new Vertex3D(meshShape.Vertices[3], Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1))
            };

            return new MapBrush()
            {
                Vertices = vertices,
                Position = center,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2,
                TriangleIndices = meshShape.TriangleIndices
            };
        }

        public static MapBrush Box(Vector3 center, float width, float height, float depth)
        {
            var meshShape = MeshShape.Box(width, height, depth);

            var uvs = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 0),
                new Vector2(2, 2)
            };
            var normals = new List<Vector3>
            {
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 1),
                new Vector3(-1, 0, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, 0, -1)
            };
            var uvIndices = new List<int>
            {
                2, 1, 3, 2, 3, 4,
                2, 1, 3, 2, 3, 4,
                2, 1, 3, 2, 3, 4,
                2, 1, 3, 2, 3, 4,
                2, 1, 3, 2, 3, 4,
                2, 1, 3, 2, 3, 4
            };
            var normalIndices = new List<int>
            {
                1, 1, 1, 1, 1, 1,
                2, 2, 2, 2, 2, 2,
                3, 3, 3, 3, 3, 3,
                4, 4, 4, 4, 4, 4,
                5, 5, 5, 5, 5, 5, 
                6, 6, 6, 6, 6, 6
            };

            var vertices = new List<Vertex3D>();
            var triangleIndices = new List<int>();
            Vector3 tangent = Vector3.Zero;

            for (var i = 0; i < meshShape.TriangleIndices.Count; i++)
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
                    var deltaPos1 = meshShape.Vertices[meshShape.TriangleIndices[i + 1] - 1] - meshShape.Vertices[meshShape.TriangleIndices[i] - 1];
                    var deltaPos2 = meshShape.Vertices[meshShape.TriangleIndices[i + 2] - 1] - meshShape.Vertices[meshShape.TriangleIndices[i] - 1];
                    var deltaUV1 = uvs[uvIndices[i + 1] - 1] - uvs[uvIndices[i] - 1];
                    var deltaUV2 = uvs[uvIndices[i + 1] - 1] - uvs[uvIndices[i] - 1];

                    var r = 1 / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y - deltaUV2.X);
                    tangent = (deltaPos1 * deltaUV2.Y - deltaPos2 * deltaUV1.Y) * r;
                }

                var uv = uvIndices[i] > 0 ? uvs[uvIndices[i] - 1] : Vector2.Zero;

                var meshVertex = new Vertex3D(meshShape.Vertices[meshShape.TriangleIndices[i] - 1], normals[normalIndices[i] - 1], tangent.Normalized(), uv);
                var existingIndex = vertices.FindIndex(v => v.Position == meshVertex.Position
                    && v.Normal == meshVertex.Normal
                    && v.TextureCoords == meshVertex.TextureCoords);

                if (existingIndex >= 0)
                {
                    triangleIndices.Add(existingIndex);
                }
                else
                {
                    triangleIndices.Add(vertices.Count);
                    vertices.Add(meshVertex);
                }
            }

            return new MapBrush()
            {
                Position = center,
                Vertices = vertices,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2,
                TriangleIndices = triangleIndices
            };
        }
    }
}
