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
    public class FaceEntity : TexturedModelEntity<ModelFace>, /*ITextureBinder, ITexturePath, */IDirectional
    {
        private Vector2 _texturePosition;
        private float _textureRotation;
        private Vector2 _textureScale;

        public override Vector3 XDirection => IsInTextureMode ? ModelShape.Tangent : Vector3.UnitX;
        public override Vector3 YDirection => IsInTextureMode ? Vector3.Cross(ModelShape.Normal, ModelShape.Tangent) : Vector3.UnitY;
        public override Vector3 ZDirection => IsInTextureMode ? Vector3.Zero : Vector3.UnitZ;

        public FaceEntity(ModelFace modelFace, TexturePaths texturePaths) : base(modelFace, texturePaths) { }

        /*public override bool CompareUniforms(IEntity entity) => entity is FaceEntity faceEntity
            && IsInTextureMode == faceEntity.IsInTextureMode
            && base.CompareUniforms(entity);*/

        public override IRenderable ToRenderable()
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
