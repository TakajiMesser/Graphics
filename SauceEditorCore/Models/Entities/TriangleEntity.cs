using SpiceEngine.Entities.Selection;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Linq;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SauceEditorCore.Models.Entities
{
    public class TriangleEntity : TexturedModelEntity<ModelTriangle>, /*ITextureBinder, ITexturePath, */IDirectional
    {
        public override Vector3 XDirection => Vector3.UnitX;
        public override Vector3 YDirection => Vector3.UnitY;
        public override Vector3 ZDirection => Vector3.UnitZ;

        public TriangleEntity(ModelTriangle modelTriangle, TexturePaths texturePaths) : base(modelTriangle, texturePaths) { }

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
