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
        
        public Mesh Mesh { get; set; }
        public List<Vertex> Vertices => Mesh.Vertices;
        public Dictionary<string, GameProperty> Properties { get; private set; } = new Dictionary<string, GameProperty>();
        public Bounds Bounds { get; set; }
        public bool HasCollision { get; set; } = true;
        public TextureMapping TextureMapping { get; set; }

        public Brush(List<Vertex> vertices, List<Material> materials, List<int> triangleIndices, ShaderProgram program)
        {
            Mesh = new Mesh(vertices, materials, triangleIndices, program);
        }

        public void AddTestColors() => Mesh.AddTestColors();

        public void AddLights(IEnumerable<Light> lights) => Mesh.AddLights(lights);

        public void Draw(ShaderProgram program)
        {
            _modelMatrix.Set(program);
            Mesh.Draw();
        }

        public static Brush Rectangle(Vector3 center, float width, float height, ShaderProgram program)
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0),
                new Vertex(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero, 0)
            };

            var materials = new List<Material>
            {
                Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2
            };

            var triangleIndices = new List<int>()
            {
                0, 1, 2, 1, 2, 3
            };

            return new Brush(vertices, materials, triangleIndices, program);
        }
    }
}
