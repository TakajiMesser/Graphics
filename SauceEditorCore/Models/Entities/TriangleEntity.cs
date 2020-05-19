using OpenTK;
using SpiceEngine.Entities.Selection;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class TriangleEntity : TexturedModelEntity<ModelTriangle>, /*ITextureBinder, ITexturePath, */IDirectional
    {
        public override Vector3 XDirection => Vector3.UnitX;
        public override Vector3 YDirection => Vector3.UnitY;
        public override Vector3 ZDirection => Vector3.UnitZ;

        public TriangleEntity(ModelTriangle modelTriangle, TexturePaths texturePaths) : base(modelTriangle, texturePaths) { }

        /*public override bool CompareUniforms(IEntity entity) => entity is TriangleEntity
            && base.CompareUniforms(entity);*/

        public override IRenderable ToComponent()
        {
            var meshBuild = new ModelBuilder(ModelShape);
            var meshVertices = meshBuild.GetVertices();

            ITexturedMesh mesh;

            if (meshVertices.Any(v => v.IsAnimated))
            {
                var vertexSet = new Vertex3DSet<AnimatedVertex3D>(meshBuild.GetVertices().Select(v => v.ToJointVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList());
                mesh = new TexturedMesh<AnimatedVertex3D>(vertexSet);
            }
            else
            {
                var vertexSet = new Vertex3DSet<Vertex3D>(meshBuild.GetVertices().Select(v => v.ToVertex3D()).ToList(), meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList());
                mesh = new TexturedMesh<Vertex3D>(vertexSet);
            }

            mesh.Material = _material;
            mesh.TextureMapping = _textureMapping;

            mesh.Transform(_modelMatrix.WorldTransform);
            return mesh;
        }
    }
}
