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
    public class Brush : Entity, IRotate, IScale, ITextureBinder
    {
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
        public TextureMapping? TextureMapping { get; set; }

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
                Scale = Scale
            };

            if (TextureMapping.HasValue)
            {
                var textureMapping = TextureMapping.Value;
                brush.TextureMapping = new TextureMapping(textureMapping.DiffuseMapID, textureMapping.NormalMapID, textureMapping.ParallaxMapID, textureMapping.SpecularMapID);
            }

            return brush;
        }

        public void BindTextures(ShaderProgram program, ITextureProvider textureProvider)
        {
            if (TextureMapping.HasValue)
            {
                program.BindTextures(textureProvider, TextureMapping.Value);
            }
            else
            {
                program.UnbindTextures();
            }
        }

        public override void SetUniforms(ShaderProgram program)
        {
            base.SetUniforms(program);
            Material.SetUniforms(program);
        }
    }
}
