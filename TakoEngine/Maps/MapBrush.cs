using OpenTK;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Helpers;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Materials;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;

namespace TakoEngine.Maps
{
    public class MapBrush
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;

        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public Material Material { get; set; }
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public bool HasCollision { get; set; }
        public TexturePaths TexturesPaths { get; set; } = new TexturePaths();

        public Brush ToBrush()
        {
            var brush = new Brush(Vertices, Material, TriangleIndices)
            {
                Position = Position,
                OriginalRotation = Rotation,
                Scale = Scale,
                HasCollision = HasCollision
            };
            brush.Bounds = new BoundingBox(brush);
            brush.AddTestColors();

            return brush;
        }

        public static MapBrush Rectangle(Vector3 center, float width, float height)
        {
            return new MapBrush()
            {
                Vertices = new List<Vertex>
                {
                    new Vertex(new Vector3(-width / 2.0f, -height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)),
                    new Vertex(new Vector3(-width / 2.0f, height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)),
                    new Vertex(new Vector3(width / 2.0f, -height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)),
                    new Vertex(new Vector3(width / 2.0f, height / 2.0f, 0.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1))
                },
                Position = center,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2,
                TriangleIndices = new List<int>()
                {
                    0, 2, 1, 1, 2, 3
                }
            };
        }

        public static MapBrush RectangularPrism(Vector3 center, float width, float height, float depth)
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

            var vertexIndices = new List<int>
            {
                8, 7, 5, 8, 5, 6,
                2, 4, 8, 2, 8, 6,
                4, 3, 7, 4, 7, 8,
                2, 1, 3, 2, 3, 4, 
                3, 1, 5, 3, 5, 7,
                6, 5, 1, 6, 1, 2
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

            var verticies = new List<Vertex>();
            var triangleIndices = new List<int>();

            Vector3 tangent = Vector3.Zero;

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
                    var deltaUV1 = uvs[uvIndices[i + 1] - 1] - uvs[uvIndices[i] - 1];
                    var deltaUV2 = uvs[uvIndices[i + 1] - 1] - uvs[uvIndices[i] - 1];

                    var r = 1 / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y - deltaUV2.X);
                    tangent = (deltaPos1 * deltaUV2.Y - deltaPos2 * deltaUV1.Y) * r;
                }

                var uv = uvIndices[i] > 0 ? uvs[uvIndices[i] - 1] : Vector2.Zero;

                var meshVertex = new Vertex(vertices[vertexIndices[i] - 1], normals[normalIndices[i] - 1], tangent.Normalized(), uv);
                var existingIndex = verticies.FindIndex(v => v.Position == meshVertex.Position
                    && v.Normal == meshVertex.Normal
                    && v.TextureCoords == meshVertex.TextureCoords);

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

            return new MapBrush()
            {
                Position = center,
                Vertices = verticies,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2,
                TriangleIndices = triangleIndices
            };

            return new MapBrush()
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
            };
        }
    }
}
