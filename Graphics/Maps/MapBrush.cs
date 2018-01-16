using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Materials;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using Graphics.Rendering.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Maps
{
    public class MapBrush
    {
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<Material> Materials { get; set; } = new List<Material>();
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public bool HasCollision { get; set; }
        public string TextureFilePath { get; set; }
        public string NormalMapFilePath { get; set; }
        public string DiffuseMapFilePath { get; set; }
        public string SpecularMapFilePath { get; set; }

        public Brush ToBrush(ShaderProgram program)
        {
            var brush = new Brush(Vertices, Materials, TriangleIndices, program)
            {
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
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0), 0),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1), 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0), 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1), 0)
                },
                Materials = new List<Material>
                {
                    Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
                },
                TriangleIndices = new List<int>()
                {
                    0, 1, 2, 1, 2, 3
                }
            };
        }

        public static MapBrush RectangularPrism(Vector3 center, float width, float height, float depth)
        {
            var vertices = new List<Vector3>
            {
                new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z - depth / 2.0f),
                new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z - depth / 2.0f),
                new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z + depth / 2.0f),
                new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z + depth / 2.0f),
                new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z - depth / 2.0f),
                new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z - depth / 2.0f),
                new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z + depth / 2.0f),
                new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z + depth / 2.0f)
            };

            var uvs = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1)
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
                7, 8, 5, 8, 5, 6,
                4, 2, 8, 2, 8, 6,
                3, 4, 7, 4, 7, 8,
                1, 2, 3, 2, 3, 4, 
                1, 3, 5, 3, 5, 7,
                5, 6, 1, 6, 1, 2
            };
            var uvIndices = new List<int>
            {
                1, 2, 3, 2, 3, 4,
                1, 2, 3, 2, 3, 4,
                1, 2, 3, 2, 3, 4,
                1, 2, 3, 2, 3, 4,
                1, 2, 3, 2, 3, 4,
                1, 2, 3, 2, 3, 4
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

                var meshVertex = new Vertex(vertices[vertexIndices[i] - 1], normals[normalIndices[i] - 1], tangent, uv, 0);
                var existingIndex = verticies.FindIndex(v => v.Position == meshVertex.Position
                    && v.Normal == meshVertex.Normal
                    && v.TextureCoords == meshVertex.TextureCoords
                    && v.MaterialIndex == meshVertex.MaterialIndex);

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
                Vertices = verticies,
                Materials = new List<Material> { Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2 },
                TriangleIndices = triangleIndices
            };

            return new MapBrush()
            {
                Vertices = new List<Vertex>
                {
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0), 0),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1), 0),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0), 0),
                    new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1), 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0), 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z - depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1), 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0), 0),
                    new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z + depth / 2.0f), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1), 0)
                },
                Materials = new List<Material>
                {
                    Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
                },
                TriangleIndices = new List<int>()
                {
                    6, 7, 4, 7, 4, 5, 3, 1, 7, 1, 7, 5, 2, 3, 6, 3, 6, 7, 0, 1, 2, 1, 2, 3, 0, 2, 4, 2, 4, 6, 4, 5, 0, 5, 0, 1
                }
            };
        }
    }
}
