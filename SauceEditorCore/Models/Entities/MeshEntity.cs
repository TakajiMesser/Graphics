using SpiceEngine.Entities;
using SpiceEngine.Helpers;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class MeshEntity : ModelEntity<ModelMesh>, ITextureBinder, ITexturePath
    {
        public TexturePaths TexturePaths { get; }
        public Material Material { get; private set; }
        public TextureMapping? TextureMapping { get; private set; }

        public MeshEntity(ModelMesh modelMesh, TexturePaths texturePaths) : base(modelMesh)
        {
            TexturePaths = texturePaths;
            Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;
        }

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
            //base.SetUniforms(program);
            base.SetUniforms(program);
            //_modelMatrix.Set(program);
            Material.SetUniforms(program);
        }

        public override bool CompareUniforms(IEntity entity) => entity is MeshEntity shapeEntity
            && Material.Equals(shapeEntity.Material)
            && TextureMapping.Equals(shapeEntity.TextureMapping);

        public override IRenderable ToRenderable()
        {
            var meshBuild = new ModelBuilder(ModelShape);
            var meshVertices = meshBuild.GetVertices();

            /*if (meshVertices.Any(v => v.IsAnimated))
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToJointVertex3D());
                return new Mesh<JointVertex3D>(vertices.ToList(), meshBuild.TriangleIndices);
            }
            else
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToVertex3D());
                return new Mesh<Vertex3D>(vertices.ToList(), meshBuild.TriangleIndices);
            }*/

            var mesh = meshVertices.Any(v => v.IsAnimated)
                ? (IMesh)new Mesh<AnimatedVertex3D>(meshBuild.GetVertices().Select(v => v.ToJointVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList())
                : new Mesh<Vertex3D>(meshBuild.GetVertices().Select(v => v.ToVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList());

            mesh.Transform(_modelMatrix.Matrix);
            return mesh;

            /*var vertices = meshVertices.Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList();
            var triangleIndices = meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList();

            mesh.Transform(_modelMatrix.Matrix);
            return new Mesh<Vertex3D>(vertices, triangleIndices);*/
        }
    }
}
