using Graphics.Lighting;
using Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using OpenTK.Graphics;
using OpenTK;
using Graphics.Physics.Collision;
using Graphics.Helpers;
using Graphics.Rendering.Vertices;
using Graphics.GameObjects;
using Graphics.Rendering.Matrices;
using Graphics.Meshes;
using Graphics.Rendering.Textures;

namespace Graphics.GameObjects
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush
    {
        private ModelMatrix _modelMatrix = new ModelMatrix();

        public SimpleModel Model { get; set; } = new SimpleModel();
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = true;
        public TextureMapping TextureMapping { get; set; }

        public Brush(List<Vertex> vertices, Material material, List<int> triangleIndices)
        {
            Model.Meshes.Add(new Mesh<Vertex>(vertices, material, triangleIndices));
            //SimpleMesh = new SimpleMesh(vertices.Select(v => v.Position).ToList(), triangleIndices, program);
        }

        public GameObject ToGameObject()
        {
            var gameObject = new GameObject("wall2")
            {
                //Mesh = Mesh,
                Bounds = Bounds,
                HasCollision = HasCollision,
                TextureMapping = TextureMapping,
                //Position = new Vector3(0, 0, 0)
            };

            gameObject.Bounds.AttachedObject = gameObject;

            return gameObject;
        }

        public void AddTestColors() => Model.AddTestColors();
        public void AddPointLights(IEnumerable<PointLight> lights) => Model.AddPointLights(lights);

        public void Draw(ShaderProgram program) => Model.Draw(program);

        public static Brush Rectangle(Vector3 center, float width, float height)
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0)
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
