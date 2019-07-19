using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class TriangleEntity : ModelEntity<ModelTriangle>, IRotate, IScale, ITextureBinder, ITexturePath
    {
        public Quaternion Rotation
        {
            // TODO - Determine if quaternion multiplication order matters here
            get => _modelMatrix.Rotation;
            set
            {
                var rotationChange = value / _modelMatrix.Rotation;
                _modelMatrix.Rotation = value;

                if (rotationChange.IsSignificant())
                {
                    Transformed?.Invoke(this, new EntityTransformEventArgs(ID, Matrix4.CreateFromQuaternion(rotationChange)));
                }
            }
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set
            {
                var scaleChange = value / _modelMatrix.Scale;
                _modelMatrix.Scale = value;

                if (scaleChange.IsSignificantDifference(Vector3.One))
                {
                    Transformed?.Invoke(this, new EntityTransformEventArgs(ID, Matrix4.CreateScale(scaleChange)));
                }
            }
        }

        public TexturePaths TexturePaths { get; }
        public Material Material { get; private set; }
        public TextureMapping? TextureMapping { get; private set; }

        public event EventHandler<TextureTransformEventArgs> TextureTransformed;

        public TriangleEntity(ModelTriangle modelTriangle, TexturePaths texturePaths) : base(modelTriangle) => TexturePaths = texturePaths;

        public void AddMaterial(Material material) => Material = material;

        public void AddTextureMapping(TextureMapping? textureMapping) => TextureMapping = textureMapping;

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

        public override bool CompareUniforms(IEntity entity) => entity is TriangleEntity triangleEntity
            && Material.Equals(triangleEntity.Material)
            && TextureMapping.Equals(triangleEntity.TextureMapping);

        public override IRenderable ToRenderable()
        {
            var meshBuild = new ModelBuilder(ModelShape);
            var meshVertices = meshBuild.GetVertices();

            var mesh = meshVertices.Any(v => v.IsAnimated)
                ? (IMesh)new Mesh<AnimatedVertex3D>(meshBuild.GetVertices().Select(v => v.ToJointVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList())
                : new Mesh<Vertex3D>(meshBuild.GetVertices().Select(v => v.ToVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList());

            mesh.Transform(_modelMatrix.Matrix);
            return mesh;
        }
    }
}
