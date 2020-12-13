using SpiceEngine.Entities.Selection;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
