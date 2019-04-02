using OpenTK;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;

namespace SpiceEngine.Entities.Brushes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Brush : IEntity, IRotate, IScale
    {
        public int ID { get; set; }

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }

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

        //public List<Vector3> Vertices => Mesh.Vertices.Select(v => v.Position).Distinct().ToList();
        public Matrix4 ModelMatrix => _modelMatrix.Matrix;

        public Material Material { get; set; }
        public TextureMapping TextureMapping { get; set; }

        private ModelMatrix _modelMatrix = new ModelMatrix();

        public Brush(Material material)
        {
            Material = material;
            //SimpleMesh = new SimpleMesh(vertices.Select(v => v.Position).ToList(), triangleIndices, program);
        }

        public Brush Duplicate()
        {
            var brush = new Brush(Material)
            {
                Position = Position,
                Rotation = Rotation,
                Scale = Scale,
                TextureMapping = new TextureMapping()
                {
                    DiffuseMapID = TextureMapping.DiffuseMapID,
                    NormalMapID = TextureMapping.NormalMapID,
                    ParallaxMapID = TextureMapping.ParallaxMapID,
                    SpecularMapID = TextureMapping.SpecularMapID
                }
            };

            return brush;
        }

        public void SetUniforms(ShaderProgram program, TextureManager textureManager = null)
        {
            _modelMatrix.Set(program);
            Material.SetUniforms(program);

            if (textureManager != null && TextureMapping != null)
            {
                program.BindTextures(textureManager, TextureMapping);
            }
            else
            {
                program.UnbindTextures();
            }
        }

        /*public static Brush Rectangle(Vector3 center, float width, float height)
        {
            var vertices = new List<Vertex3D>
            {
                new Vertex3D(new Vector3(center.X - width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero),
                new Vertex3D(new Vector3(center.X - width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero),
                new Vertex3D(new Vector3(center.X + width / 2.0f, center.Y - height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero),
                new Vertex3D(new Vector3(center.X + width / 2.0f, center.Y + height / 2.0f, center.Z), Vector3.UnitZ, Vector3.UnitY, Vector2.Zero)
            };

            var material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;

            var triangleIndices = new List<int>()
            {
                0, 1, 2, 1, 2, 3
            };

            return new Brush(vertices, material, triangleIndices);
        }*/
    }
}
