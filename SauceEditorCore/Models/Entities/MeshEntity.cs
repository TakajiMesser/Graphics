using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Selection;
using SpiceEngine.Helpers;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class MeshEntity : TexturedModelEntity<ModelMesh>, ITextureBinder, ITexturePath, IDirectional
    {
        public override Vector3 XDirection => Vector3.UnitX;
        public override Vector3 YDirection => Vector3.UnitY;
        public override Vector3 ZDirection => Vector3.UnitZ;

        public MeshEntity(ModelMesh modelMesh, TexturePaths texturePaths) : base(modelMesh, texturePaths) { }

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

            mesh.Transform(_modelMatrix.WorldTransform);
            return mesh;

            /*var vertices = meshVertices.Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList();
            var triangleIndices = meshBuild.TriangleIndices.AsEnumerable().Reverse().ToList();

            mesh.Transform(_modelMatrix.Matrix);
            return new Mesh<Vertex3D>(vertices, triangleIndices);*/
        }
    }
}
