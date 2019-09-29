using SauceEditorCore.Helpers;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Textures;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;

namespace SauceEditorCore.Models.Entities
{
    public class VertexEntity : ModelEntity<ModelVertex>
    {
        public VertexEntity(ModelVertex meshVertex) : base(meshVertex) { }

        public override bool CompareUniforms(IEntity entity) => entity is VertexEntity;

        public override IRenderable ToRenderable()
        {
            return new TextureID(FilePathHelper.VERTEX_TEXTURE_PATH)
            {
                Position = Position
            };
            /*var meshBuild = new MeshBuild(Triangle);
            var meshVertices = meshBuild.GetVertices();

            if (meshVertices.Any(v => v.IsAnimated))
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToVertex3D());
                return new Mesh<Vertex3D>(vertices, meshBuild.TriangleIndices);
            }
            else
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToJointVertex3D());
                return new Mesh<JointVertex3D>(vertices, meshBuild.TriangleIndices);
            }*/
        }
    }
}
