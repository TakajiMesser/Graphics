using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities.Lights;
using TakoEngine.Game;
using TakoEngine.Helpers;
using TakoEngine.Physics.Collision;
using TakoEngine.Rendering.Materials;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Meshes;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Scripting.StimResponse;

namespace TakoEngine.Entities
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush : IEntity, IStimulate, ICollidable, IRotate, IScale
    {
        public int ID { get; set; }

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

        public Vector3 OriginalRotation { get; set; }

        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }

        public Mesh<Vertex> Mesh { get; private set; }
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();

        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = true;
        public List<Vector3> Vertices => Mesh.Vertices.Select(v => v.Position).Distinct().ToList();

        private ModelMatrix _modelMatrix = new ModelMatrix();

        public Brush(List<Vertex> vertices, Material material, List<int> triangleIndices)
        {
            Mesh = new Mesh<Vertex>(vertices, material, triangleIndices);
            //SimpleMesh = new SimpleMesh(vertices.Select(v => v.Position).ToList(), triangleIndices, program);
        }

        //public void ClearLights() => Mesh.ClearLights();
        //public void AddPointLights(IEnumerable<PointLight> lights) => Mesh.AddPointLights(lights);

        public void AddTestColors()
        {
            var vertices = new List<Vertex>();

            for (var i = 0; i < Mesh.Vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Lime));
                }
                else if (i % 3 == 1)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Red));
                }
                else if (i % 3 == 2)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Blue));
                }
            }

            Mesh.ClearVertices();
            Mesh.AddVertices(vertices);
            //Mesh.RefreshVertices();
        }

        public void Load() => Mesh.Load();

        public void Draw(ShaderProgram program, TextureManager textureManager = null)
        {
            _modelMatrix.Set(program);
            Mesh.Draw(program, textureManager);
        }

        public static Brush Rectangle(Vector3 center, float width, float height)
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero),
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero)
            };

            var material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;

            var triangleIndices = new List<int>()
            {
                0, 1, 2, 1, 2, 3
            };

            return new Brush(vertices, material, triangleIndices);
        }
    }
}
