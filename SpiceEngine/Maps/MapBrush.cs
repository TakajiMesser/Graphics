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
    public class MapBrush : MapEntity3D<Brush>
    {
        public List<Vertex3D> Vertices { get; set; } = new List<Vertex3D>();
        public Material Material { get; set; }
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public bool IsPhysical { get; set; }
        public TexturePaths TexturesPaths { get; set; } = new TexturePaths();

        public MapBrush() { }
        public MapBrush(MeshBuild meshBuild)
        {
            Vertices = meshBuild.Vertices.Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList();
            TriangleIndices = meshBuild.TriangleIndices;
            Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;
        }

        public Mesh<Vertex3D> ToMesh()
        {
            if (TexturesPaths.IsEmpty)
            {
                AddTestColors();
            }
            
            return new Mesh<Vertex3D>(Vertices, TriangleIndices);
        }

        public override Brush ToEntity() => new Brush(Material)
        {
            Position = Position,
            Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians()),
            Scale = Scale,
            //HasCollision = HasCollision
        };

        public Shape3D ToShape() => new Box(Vertices.Select(v => v.Position));

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
            var meshShape = new MeshShape();
            meshShape.Faces.Add(MeshFace.Rectangle(width, height));
            var meshBuild = meshShape.GetMeshBuild();

            return new MapBrush()
            {
                Position = center,
                Vertices = meshBuild.Vertices.Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList(),
                TriangleIndices = meshBuild.TriangleIndices,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
            };

            /*var meshShape = MeshShape.Rectangle(width, height);

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
            };*/
        }

        public static MapBrush Box(Vector3 center, float width, float height, float depth)
        {
            var meshShape = MeshShape.Box(width, height, depth);
            var meshBuild = meshShape.GetMeshBuild();

            return new MapBrush()
            {
                Position = center,
                Vertices = meshBuild.Vertices.Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList(),
                TriangleIndices = meshBuild.TriangleIndices,
                Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
            };
            
            /*var uvs = new List<Vector2>
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
            };*/
        }

        /*public static MapBrush Sphere(Vector3 center, float radius)
        {
            var meshShape = MeshShape.Sphere(radius);

            var uvs = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(2, 0),
                new Vector2(2, 2)
            };
            var normals = new List<Vector3>
            {
                new Vector3(0.0482f, -0.9878f, 0.1482f),
                new Vector3(0.7230f, -0.5545f, 0.4121f),
                new Vector3(-0.1261f, -0.9878f, 0.0916f),
                new Vector3(-0.1261f, -0.9878f, -0.0916f),
                new Vector3(0.0482f, -0.9878f, -0.1482f),
                new Vector3(0.8193f, -0.3987f, 0.4121f),
                new Vector3(-0.1387f, -0.3987f, 0.9065f),
                new Vector3(-0.9050f, -0.3987f, 0.1482f),
                new Vector3(-0.4206f, -0.3987f, -0.8149f),
                new Vector3(0.6451f, -0.3987f, -0.6519f),
                new Vector3(0.7711f, -0.3024f, 0.5603f),
                new Vector3(-0.2945f, -0.3024f, 0.9065f),
                new Vector3(-0.9532f, -0.3024f, 0.0000f),
                new Vector3(-0.2945f, -0.3024f, -0.9065f),
                new Vector3(0.7711f, -0.3024f, -0.5603f),
                new Vector3(0.3427f, 0.5545f, 0.7583f),
                new Vector3(-0.6153f, 0.5545f, 0.5603f),
                new Vector3(-0.7230f, 0.5545f, -0.4121f),
                new Vector3(0.1685f, 0.5545f, -0.8149f),
                new Vector3(0.8271f, 0.5545f, -0.0916f),
                new Vector3(0.1261f, 0.9878f, -0.0916f),
                new Vector3(0.3912f, 0.9158f, -0.0914f),
                new Vector3(0.6441f, 0.7594f, -0.0915f),
                new Vector3(0.2334f, 0.9575f, -0.1696f),
                new Vector3(0.2079f, 0.9158f, -0.3438f),
                new Vector3(0.5168f, 0.8363f, -0.1830f),
                new Vector3(0.4911f, 0.7947f, -0.3568f),
                new Vector3(0.3338f, 0.8363f, -0.4350f),
                new Vector3(0.2860f, 0.7594f, -0.5843f),
                new Vector3(0.7520f, 0.6369f, -0.1696f),
                new Vector3(0.7261f, 0.5955f, -0.3438f),
                new Vector3(0.5987f, 0.6726f, -0.4350f),
                new Vector3(0.5513f, 0.5955f, -0.5843f),
                new Vector3(0.3936f, 0.6369f, -0.6628f),
                new Vector3(0.3427f, 0.5545f, -0.7583f),
                new Vector3(-0.0482f, 0.9878f, -0.1482f),
                new Vector3(0.0339f, 0.9158f, -0.4003f),
                new Vector3(0.1121f, 0.7594f, -0.6409f),
                new Vector3(-0.0891f, 0.9575f, -0.2743f),
                new Vector3(-0.2627f, 0.9158f, -0.3039f),
                new Vector3(-0.0144f, 0.8363f, -0.5481f),
                new Vector3(-0.1876f, 0.7947f, -0.5773f),
                new Vector3(-0.3105f, 0.8363f, -0.4519f),
                new Vector3(-0.4674f, 0.7594f, -0.4526f),
                new Vector3(0.0711f, 0.6369f, -0.7676f),
                new Vector3(-0.1026f, 0.5955f, -0.7968f),
                new Vector3(-0.2287f, 0.6726f, -0.7038f),
                new Vector3(-0.3854f, 0.5955f, -0.7049f),
                new Vector3(-0.5087f, 0.6369f, -0.5792f),
                new Vector3(-0.6153f, 0.5545f, -0.5603f),
                new Vector3(-0.1558f, 0.9878f, 0.0000f),
                new Vector3(-0.3702f, 0.9158f, -0.1560f),
                new Vector3(-0.5749f, 0.7594f, -0.3046f),
                new Vector3(-0.2885f, 0.9575f, 0.0000f),
                new Vector3(-0.3702f, 0.9158f, 0.1560f),
                new Vector3(-0.5257f, 0.8363f, -0.1557f),
                new Vector3(-0.6071f, 0.7947f, 0.0000f),
                new Vector3(-0.5257f, 0.8363f, 0.1557f),
                new Vector3(-0.5749f, 0.7594f, 0.3046f),
                new Vector3(-0.7081f, 0.6369f, -0.3049f),
                new Vector3(-0.7895f, 0.5955f, -0.1487f),
                new Vector3(-0.7400f, 0.6726f, 0.0000f),
                new Vector3(-0.7895f, 0.5955f, 0.1487f),
                new Vector3(-0.7081f, 0.6369f, 0.3049f),
                new Vector3(-0.7230f, 0.5545f, 0.4121f),
                new Vector3(-0.0482f, 0.9878f, 0.1482f),
                new Vector3(-0.2627f, 0.9158f, 0.3039f),
                new Vector3(-0.4674f, 0.7594f, 0.4526f),
                new Vector3(-0.0891f, 0.9575f, 0.2743f),
                new Vector3(0.0339f, 0.9158f, 0.4003f),
                new Vector3(-0.3105f, 0.8363f, 0.4519f),
                new Vector3(-0.1876f, 0.7947f, 0.5773f),
                new Vector3(-0.0144f, 0.8363f, 0.5481f),
                new Vector3(0.1121f, 0.7594f, 0.6409f),
                new Vector3(-0.5087f, 0.6369f, 0.5792f),
                new Vector3(-0.3854f, 0.5955f, 0.7049f),
                new Vector3(-0.2287f, 0.6726f, 0.7038f),
                new Vector3(-0.1026f, 0.5955f, 0.7968f),
                new Vector3(0.0711f, 0.6369f, 0.7676f),
                new Vector3(0.1685f, 0.5545f, 0.8149f),
                new Vector3(0.1261f, 0.9878f, 0.0916f),
                new Vector3(0.2079f, 0.9158f, 0.3438f),
                new Vector3(0.2860f, 0.7594f, 0.5843f),
                new Vector3(0.2334f, 0.9575f, 0.1696f),
                new Vector3(0.3912f, 0.9158f, 0.0915f),
                new Vector3(0.3338f, 0.8363f, 0.4350f),
                new Vector3(0.4911f, 0.7947f, 0.3568f),
                new Vector3(0.5168f, 0.8363f, 0.1830f),
                new Vector3(0.6441f, 0.7594f, 0.0915f),
                new Vector3(0.3936f, 0.6369f, 0.6628f),
                new Vector3(0.5513f, 0.5955f, 0.5843f),
                new Vector3(0.5987f, 0.6726f, 0.4350f),
                new Vector3(0.7261f, 0.5955f, 0.3438f),
                new Vector3(0.7520f, 0.6369f, 0.1696f),
                new Vector3(0.8271f, 0.5545f, 0.0916f),
                new Vector3(0.9050f, 0.3987f, -0.1482f),
                new Vector3(0.9366f, 0.1745f, -0.3039f),
                new Vector3(0.8883f, -0.0784f, -0.4526f),
                new Vector3(0.8963f, 0.3485f, -0.2743f),
                new Vector3(0.8039f, 0.4399f, -0.4003f),
                new Vector3(0.8869f, 0.0962f, -0.4519f),
                new Vector3(0.7947f, 0.1876f, -0.5773f),
                new Vector3(0.7544f, 0.3611f, -0.5481f),
                new Vector3(0.6291f, 0.4399f, -0.6409f),
                new Vector3(0.7972f, -0.1702f, -0.5792f),
                new Vector3(0.7049f, -0.0784f, -0.7049f),
                new Vector3(0.7038f, 0.0962f, -0.7038f),
                new Vector3(0.5785f, 0.1745f, -0.7968f),
                new Vector3(0.5379f, 0.3485f, -0.7676f),
                new Vector3(0.4206f, 0.3987f, -0.8149f),
                new Vector3(0.1387f, 0.3987f, -0.9065f),
                new Vector3(0.0004f, 0.1745f, -0.9846f),
                new Vector3(-0.1560f, -0.0784f, -0.9846f),
                new Vector3(0.0160f, 0.3485f, -0.9372f),
                new Vector3(-0.1323f, 0.4399f, -0.8883f),
                new Vector3(-0.1557f, 0.0962f, -0.9831f),
                new Vector3(-0.3035f, 0.1876f, -0.9342f),
                new Vector3(-0.2882f, 0.3611f, -0.8869f),
                new Vector3(-0.4151f, 0.4399f, -0.7964f),
                new Vector3(-0.3045f, -0.1702f, -0.9372f),
                new Vector3(-0.4526f, -0.0784f, -0.8883f),
                new Vector3(-0.4519f, 0.0962f, -0.8869f),
                new Vector3(-0.5791f, 0.1745f, -0.7964f),
                new Vector3(-0.5638f, 0.3485f, -0.7488f),
                new Vector3(-0.6451f, 0.3987f, -0.6519f),
                new Vector3(-0.8193f, 0.3987f, -0.4121f),
                new Vector3(-0.9363f, 0.1745f, -0.3046f),
                new Vector3(-0.9846f, -0.0784f, -0.1560f),
                new Vector3(-0.8864f, 0.3485f, -0.3049f),
                new Vector3(-0.8857f, 0.4399f, -0.1487f),
                new Vector3(-0.9831f, 0.0962f, -0.1557f),
                new Vector3(-0.9822f, 0.1876f, 0.0000f),
                new Vector3(-0.9325f, 0.3611f, 0.0000f),
                new Vector3(-0.8857f, 0.4399f, 0.1487f),
                new Vector3(-0.9854f, -0.1702f, 0.0000f),
                new Vector3(-0.9846f, -0.0784f, 0.1560f),
                new Vector3(-0.9831f, 0.0962f, 0.1557f),
                new Vector3(-0.9363f, 0.1745f, 0.3046f),
                new Vector3(-0.8864f, 0.3485f, 0.3049f),
                new Vector3(-0.8193f, 0.3987f, 0.4121f),
                new Vector3(-0.6451f, 0.3987f, 0.6519f),
                new Vector3(-0.5791f, 0.1745f, 0.7964f),
                new Vector3(-0.4526f, -0.0784f, 0.8883f),
                new Vector3(-0.5638f, 0.3485f, 0.7488f),
                new Vector3(-0.4151f, 0.4399f, 0.7964f),
                new Vector3(-0.4519f, 0.0962f, 0.8869f),
                new Vector3(-0.3035f, 0.1876f, 0.9342f),
                new Vector3(-0.2882f, 0.3611f, 0.8869f),
                new Vector3(-0.1323f, 0.4399f, 0.8883f),
                new Vector3(-0.3045f, -0.1702f, 0.9372f),
                new Vector3(-0.1560f, -0.0784f, 0.9846f),
                new Vector3(-0.1557f, 0.0962f, 0.9831f),
                new Vector3(0.0004f, 0.1745f, 0.9846f),
                new Vector3(0.0160f, 0.3485f, 0.9372f),
                new Vector3(0.1387f, 0.3987f, 0.9065f),
                new Vector3(0.4206f, 0.3987f, 0.8149f),
                new Vector3(0.5785f, 0.1745f, 0.7968f),
                new Vector3(0.7049f, -0.0784f, 0.7049f),
                new Vector3(0.5379f, 0.3485f, 0.7676f),
                new Vector3(0.6291f, 0.4399f, 0.6409f),
                new Vector3(0.7038f, 0.0962f, 0.7038f),
                new Vector3(0.7947f, 0.1876f, 0.5773f),
                new Vector3(0.7544f, 0.3611f, 0.5481f),
                new Vector3(0.8039f, 0.4399f, 0.4003f),
                new Vector3(0.7972f, -0.1702f, 0.5792f),
                new Vector3(0.8883f, -0.0784f, 0.4526f),
                new Vector3(0.8869f, 0.0962f, 0.4519f),
                new Vector3(0.9366f, 0.1745f, 0.3039f),
                new Vector3(0.8963f, 0.3485f, 0.2744f),
                new Vector3(0.9050f, 0.3987f, 0.1482f),
                new Vector3(0.2945f, 0.3024f, -0.9065f),
                new Vector3(0.4526f, 0.0784f, -0.8883f),
                new Vector3(0.5791f, -0.1745f, -0.7964f),
                new Vector3(0.3045f, 0.1702f, -0.9372f),
                new Vector3(0.1560f, 0.0784f, -0.9846f),
                new Vector3(0.4519f, -0.0962f, -0.8869f),
                new Vector3(0.3035f, -0.1876f, -0.9342f),
                new Vector3(0.1557f, -0.0962f, -0.9831f),
                new Vector3(-0.0004f, -0.1745f, -0.9846f),
                new Vector3(0.5638f, -0.3485f, -0.7488f),
                new Vector3(0.4151f, -0.4399f, -0.7964f),
                new Vector3(0.2882f, -0.3611f, -0.8869f),
                new Vector3(0.1323f, -0.4399f, -0.8883f),
                new Vector3(-0.0160f, -0.3485f, -0.9372f),
                new Vector3(-0.1387f, -0.3987f, -0.9065f),
                new Vector3(-0.7711f, 0.3024f, -0.5603f),
                new Vector3(-0.7049f, 0.0784f, -0.7049f),
                new Vector3(-0.5785f, -0.1745f, -0.7968f),
                new Vector3(-0.7972f, 0.1702f, -0.5792f),
                new Vector3(-0.8883f, 0.0784f, -0.4526f),
                new Vector3(-0.7038f, -0.0962f, -0.7038f),
                new Vector3(-0.7947f, -0.1876f, -0.5773f),
                new Vector3(-0.8869f, -0.0962f, -0.4519f),
                new Vector3(-0.9366f, -0.1745f, -0.3039f),
                new Vector3(-0.5379f, -0.3485f, -0.7676f),
                new Vector3(-0.6291f, -0.4399f, -0.6409f),
                new Vector3(-0.7544f, -0.3611f, -0.5481f),
                new Vector3(-0.8039f, -0.4399f, -0.4003f),
                new Vector3(-0.8963f, -0.3485f, -0.2744f),
                new Vector3(-0.9050f, -0.3987f, -0.1482f),
                new Vector3(-0.7711f, 0.3024f, 0.5603f),
                new Vector3(-0.8883f, 0.0784f, 0.4526f),
                new Vector3(-0.9366f, -0.1745f, 0.3039f),
                new Vector3(-0.7972f, 0.1702f, 0.5792f),
                new Vector3(-0.7049f, 0.0784f, 0.7049f),
                new Vector3(-0.8869f, -0.0962f, 0.4519f),
                new Vector3(-0.7947f, -0.1876f, 0.5774f),
                new Vector3(-0.7038f, -0.0962f, 0.7038f),
                new Vector3(-0.5785f, -0.1745f, 0.7968f),
                new Vector3(-0.8963f, -0.3485f, 0.2744f),
                new Vector3(-0.8039f, -0.4399f, 0.4003f),
                new Vector3(-0.7544f, -0.3611f, 0.5481f),
                new Vector3(-0.6291f, -0.4399f, 0.6409f),
                new Vector3(-0.5379f, -0.3485f, 0.7676f),
                new Vector3(-0.4206f, -0.3987f, 0.8149f),
                new Vector3(0.2945f, 0.3024f, 0.9065f),
                new Vector3(0.1560f, 0.0784f, 0.9846f),
                new Vector3(-0.0004f, -0.1745f, 0.9846f),
                new Vector3(0.3045f, 0.1702f, 0.9372f),
                new Vector3(0.4526f, 0.0784f, 0.8883f),
                new Vector3(0.1557f, -0.0962f, 0.9831f),
                new Vector3(0.3035f, -0.1876f, 0.9342f),
                new Vector3(0.4519f, -0.0962f, 0.8869f),
                new Vector3(0.5791f, -0.1745f, 0.7964f),
                new Vector3(-0.0160f, -0.3485f, 0.9372f),
                new Vector3(0.1323f, -0.4399f, 0.8883f),
                new Vector3(0.2882f, -0.3611f, 0.8869f),
                new Vector3(0.4151f, -0.4399f, 0.7964f),
                new Vector3(0.5638f, -0.3485f, 0.7488f),
                new Vector3(0.6451f, -0.3987f, 0.6519f),
                new Vector3(0.9532f, 0.3024f, 0.0000f),
                new Vector3(0.9846f, 0.0784f, 0.1560f),
                new Vector3(0.9363f, -0.1745f, 0.3046f),
                new Vector3(0.9854f, 0.1702f, 0.0000f),
                new Vector3(0.9846f, 0.0784f, -0.1560f),
                new Vector3(0.9831f, -0.0962f, 0.1557f),
                new Vector3(0.9822f, -0.1876f, 0.0000f),
                new Vector3(0.9831f, -0.0962f, -0.1557f),
                new Vector3(0.9363f, -0.1745f, -0.3046f),
                new Vector3(0.8864f, -0.3485f, 0.3049f),
                new Vector3(0.8857f, -0.4399f, 0.1487f),
                new Vector3(0.9325f, -0.3611f, 0.0000f),
                new Vector3(0.8857f, -0.4399f, -0.1487f),
                new Vector3(0.8864f, -0.3485f, -0.3049f),
                new Vector3(0.8193f, -0.3987f, -0.4121f),
                new Vector3(0.6153f, -0.5545f, -0.5603f),
                new Vector3(0.4674f, -0.7594f, -0.4526f),
                new Vector3(0.2627f, -0.9158f, -0.3039f),
                new Vector3(0.5087f, -0.6369f, -0.5792f),
                new Vector3(0.3854f, -0.5955f, -0.7049f),
                new Vector3(0.3105f, -0.8363f, -0.4519f),
                new Vector3(0.1876f, -0.7947f, -0.5773f),
                new Vector3(0.2287f, -0.6726f, -0.7038f),
                new Vector3(0.1026f, -0.5955f, -0.7968f),
                new Vector3(0.0891f, -0.9575f, -0.2743f),
                new Vector3(-0.0339f, -0.9158f, -0.4003f),
                new Vector3(0.0144f, -0.8363f, -0.5481f),
                new Vector3(-0.1121f, -0.7594f, -0.6409f),
                new Vector3(-0.0711f, -0.6369f, -0.7676f),
                new Vector3(-0.1685f, -0.5545f, -0.8149f),
                new Vector3(-0.3427f, -0.5545f, -0.7583f),
                new Vector3(-0.2860f, -0.7594f, -0.5843f),
                new Vector3(-0.2079f, -0.9158f, -0.3438f),
                new Vector3(-0.3936f, -0.6369f, -0.6628f),
                new Vector3(-0.5513f, -0.5955f, -0.5843f),
                new Vector3(-0.3338f, -0.8363f, -0.4350f),
                new Vector3(-0.4911f, -0.7947f, -0.3568f),
                new Vector3(-0.5987f, -0.6726f, -0.4350f),
                new Vector3(-0.7261f, -0.5955f, -0.3438f),
                new Vector3(-0.2334f, -0.9575f, -0.1696f),
                new Vector3(-0.3912f, -0.9158f, -0.0914f),
                new Vector3(-0.5168f, -0.8363f, -0.1830f),
                new Vector3(-0.6441f, -0.7594f, -0.0914f),
                new Vector3(-0.7520f, -0.6369f, -0.1696f),
                new Vector3(-0.8271f, -0.5545f, -0.0916f),
                new Vector3(-0.8271f, -0.5545f, 0.0916f),
                new Vector3(-0.6441f, -0.7594f, 0.0915f),
                new Vector3(-0.3912f, -0.9158f, 0.0915f),
                new Vector3(-0.7520f, -0.6369f, 0.1696f),
                new Vector3(-0.7261f, -0.5955f, 0.3438f),
                new Vector3(-0.5168f, -0.8363f, 0.1830f),
                new Vector3(-0.4911f, -0.7947f, 0.3568f),
                new Vector3(-0.5987f, -0.6726f, 0.4350f),
                new Vector3(-0.5513f, -0.5955f, 0.5843f),
                new Vector3(-0.2334f, -0.9575f, 0.1696f),
                new Vector3(-0.2079f, -0.9158f, 0.3438f),
                new Vector3(-0.3338f, -0.8363f, 0.4350f),
                new Vector3(-0.2860f, -0.7594f, 0.5843f),
                new Vector3(-0.3936f, -0.6369f, 0.6628f),
                new Vector3(-0.3427f, -0.5545f, 0.7583f),
                new Vector3(0.7230f, -0.5545f, -0.4121f),
                new Vector3(0.7895f, -0.5955f, -0.1487f),
                new Vector3(0.7895f, -0.5955f, 0.1487f),
                new Vector3(0.7081f, -0.6369f, -0.3049f),
                new Vector3(0.5749f, -0.7594f, -0.3046f),
                new Vector3(0.7400f, -0.6726f, -0.0000f),
                new Vector3(0.6071f, -0.7947f, 0.0000f),
                new Vector3(0.5257f, -0.8363f, -0.1557f),
                new Vector3(0.3702f, -0.9158f, -0.1559f),
                new Vector3(0.7081f, -0.6369f, 0.3049f),
                new Vector3(0.5749f, -0.7594f, 0.3046f),
                new Vector3(0.5257f, -0.8363f, 0.1557f),
                new Vector3(0.3702f, -0.9158f, 0.1560f),
                new Vector3(0.2885f, -0.9575f, 0.0000f),
                new Vector3(0.1558f, -0.9878f, 0.0000f),
                new Vector3(-0.1685f, -0.5545f, 0.8149f),
                new Vector3(-0.1121f, -0.7594f, 0.6409f),
                new Vector3(-0.0339f, -0.9158f, 0.4003f),
                new Vector3(-0.0711f, -0.6369f, 0.7676f),
                new Vector3(0.1026f, -0.5955f, 0.7968f),
                new Vector3(0.0144f, -0.8363f, 0.5481f),
                new Vector3(0.1876f, -0.7947f, 0.5773f),
                new Vector3(0.2287f, -0.6726f, 0.7038f),
                new Vector3(0.3854f, -0.5955f, 0.7049f),
                new Vector3(0.0891f, -0.9575f, 0.2744f),
                new Vector3(0.2627f, -0.9158f, 0.3039f),
                new Vector3(0.3105f, -0.8363f, 0.4519f),
                new Vector3(0.4674f, -0.7594f, 0.4526f),
                new Vector3(0.5087f, -0.6369f, 0.5792f),
                new Vector3(0.6153f, -0.5545f, 0.5603f)
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
                1, 1, 1,
                2, 2, 2,
                3, 3, 3,
                4, 4, 4,
                5, 5, 5,
                6, 6, 6,
                7, 7, 7,
                8, 8, 8,
                9, 9, 9,
                10, 10, 10,
                11, 11, 11,
                12, 12, 12,
                13, 13, 13,
                14, 14, 14,
                15, 15, 15,
                16, 16, 16,
                17, 17, 17,
                18, 18, 18,
                19, 19, 19,
                20, 20, 20,
                21, 21, 21,
                22, 22, 22,
                23, 23, 23,
                24, 24, 24,
                25, 25, 25,
                26, 26, 26,
                27, 27, 27,
                28, 28, 28,
                29, 29, 29,
                30, 30, 30,
                31, 31, 31,
                32, 32, 32,
                33, 33, 33,
                34, 34, 34,
                35, 35, 35,
                36, 36, 36,
                37, 37, 37,
                38, 38, 38,
                39, 39, 39,
                40, 40, 40,
                41, 41, 41,
                42, 42, 42,
                43, 43, 43,
                44, 44, 44,
                45, 45, 45,
                46, 46, 46,
                47, 47, 47,
                48, 48, 48,
                49, 49, 49,
                50, 50, 50,
                51, 51, 51,
                52, 52, 52,
                53, 53, 53,
                54, 54, 54,
                55, 55, 55,
                56, 56, 56,
                57, 57, 57,
                58, 58, 58,
                59, 59, 59,
                60, 60, 60,
                61, 61, 61,
                62, 62, 62,
                63, 63, 63,
                64, 64, 64,
                65, 65, 65,
                66, 66, 66,
                67, 67, 67,
                68, 68, 68,
                69, 69, 69,
                70, 70, 70,
                71, 71, 71,
                72, 72, 72,
                73, 73, 73,
                74, 74, 74,
                75, 75, 75,
                76, 76, 76,
                77, 77, 77,
                78, 78, 78,
                79, 79, 79,
                80, 80, 80,
                81, 81, 81,
                82, 82, 82,
                83, 83, 83,
                84, 84, 84,
                85, 85, 85,
                86, 86, 86,
                87, 87, 87,
                88, 88, 88,
                89, 89, 89,
                90, 90, 90,
                91, 91, 91,
                92, 92, 92,
                93, 93, 93,
                94, 94, 94,
                95, 95, 95,
                96, 96, 96,
                97, 97, 97,
                98, 98, 98,
                99, 99, 99,
                100, 100, 100,
                101, 101, 101,
                102, 102, 102,
                103, 103, 103,
                104, 104, 104,
                105, 105, 105,
                106, 106, 106,
                107, 107, 107,
                108, 108, 108,
                109, 109, 109,
                110, 110, 110,
                111, 111, 111,
                112, 112, 112,
                113, 113, 113,
                114, 114, 114,
                115, 115, 115,
                116, 116, 116,
                117, 117, 117,
                118, 118, 118,
                119, 119, 119,
                120, 120, 120,
                121, 121, 121,
                122, 122, 122,
                123, 123, 123,
                124, 124, 124,
                125, 125, 125,
                126, 126, 126,
                127, 127, 127,
                128, 128, 128,
                129, 129, 129,
                130, 130, 130,
                131, 131, 131,
                132, 132, 132,
                133, 133, 133,
                134, 134, 134,
                135, 135, 135,
                136, 136, 136,
                137, 137, 137,
                138, 138, 138,
                139, 139, 139,
                140, 140, 140,
                141, 141, 141,
                142, 142, 142,
                143, 143, 143,
                144, 144, 144,
                145, 145, 145,
                146, 146, 146,
                147, 147, 147,
                148, 148, 148,
                149, 149, 149,
                150, 150, 150,
                151, 151, 151,
                152, 152, 152,
                153, 153, 153,
                154, 154, 154,
                155, 155, 155,
                156, 156, 156,
                157, 157, 157,
                158, 158, 158,
                159, 159, 159,
                160, 160, 160,
                161, 161, 161,
                162, 162, 162,
                163, 163, 163,
                164, 164, 164,
                165, 165, 165,
                166, 166, 166,
                167, 167, 167,
                168, 168, 168,
                169, 169, 169,
                170, 170, 170,
                171, 171, 171,
                172, 172, 172,
                173, 173, 173,
                174, 174, 174,
                175, 175, 175,
                176, 176, 176,
                177, 177, 177,
                178, 178, 178,
                179, 179, 179,
                180, 180, 180,
                181, 181, 181,
                182, 182, 182,
                183, 183, 183,
                184, 184, 184,
                185, 185, 185,
                186, 186, 186,
                187, 187, 187,
                188, 188, 188,
                189, 189, 189,
                190, 190, 190,
                191, 191, 191,
                192, 192, 192,
                193, 193, 193,
                194, 194, 194,
                195, 195, 195,
                196, 196, 196,
                197, 197, 197,
                198, 198, 198,
                199, 199, 199,
                200, 200, 200,
                201, 201, 201,
                202, 202, 202,
                203, 203, 203,
                204, 204, 204,
                205, 205, 205,
                206, 206, 206,
                207, 207, 207,
                208, 208, 208,
                209, 209, 209,
                210, 210, 210,
                211, 211, 211,
                212, 212, 212,
                213, 213, 213,
                214, 214, 214,
                215, 215, 215,
                216, 216, 216,
                217, 217, 217,
                218, 218, 218,
                219, 219, 219,
                220, 220, 220,
                221, 221, 221,
                222, 222, 222,
                223, 223, 223,
                224, 224, 224,
                225, 225, 225,
                226, 226, 226,
                227, 227, 227,
                228, 228, 228,
                229, 229, 229,
                230, 230, 230,
                231, 231, 231,
                232, 232, 232,
                233, 233, 233,
                234, 234, 234,
                235, 235, 235,
                236, 236, 236,
                237, 237, 237,
                238, 238, 238,
                239, 239, 239,
                240, 240, 240,
                241, 241, 241,
                242, 242, 242,
                243, 243, 243,
                244, 244, 244,
                245, 245, 245,
                246, 246, 246,
                247, 247, 247,
                248, 248, 248,
                249, 249, 249,
                250, 250, 250,
                251, 251, 251,
                252, 252, 252,
                253, 253, 253,
                254, 254, 254,
                255, 255, 255,
                256, 256, 256,
                257, 257, 257,
                258, 258, 258,
                259, 259, 259,
                260, 260, 260,
                261, 261, 261,
                262, 262, 262,
                263, 263, 263,
                264, 264, 264,
                265, 265, 265,
                266, 266, 266,
                267, 267, 267,
                268, 268, 268,
                269, 269, 269,
                270, 270, 270,
                271, 271, 271,
                272, 272, 272,
                273, 273, 273,
                274, 274, 274,
                275, 275, 275,
                276, 276, 276,
                277, 277, 277,
                278, 278, 278,
                279, 279, 279,
                280, 280, 280,
                281, 281, 281,
                282, 282, 282,
                283, 283, 283,
                284, 284, 284,
                285, 285, 285,
                286, 286, 286,
                287, 287, 287,
                288, 288, 288,
                289, 289, 289,
                290, 290, 290,
                291, 291, 291,
                292, 292, 292,
                293, 293, 293,
                294, 294, 294,
                295, 295, 295,
                296, 296, 296,
                297, 297, 297,
                298, 298, 298,
                299, 299, 299,
                300, 300, 300,
                301, 301, 301,
                302, 302, 302,
                303, 303, 303,
                304, 304, 304,
                305, 305, 305,
                306, 306, 306,
                307, 307, 307,
                308, 308, 308,
                309, 309, 309,
                310, 310, 310,
                311, 311, 311,
                312, 312, 312,
                313, 313, 313,
                314, 314, 314,
                315, 315, 315,
                316, 316, 316,
                317, 317, 317,
                318, 318, 318,
                319, 319, 319,
                320, 320, 320
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
            }; *
        }*/
    }
}
