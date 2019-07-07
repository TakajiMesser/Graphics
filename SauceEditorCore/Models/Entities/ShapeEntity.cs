using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Helpers;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class ShapeEntity : ModelEntity
    {
        public MeshShape Shape { get; }
        public Material Material { get; set; }

        public ShapeEntity(MeshShape meshShape)
        {
            Shape = meshShape;
            Material = Material.LoadFromFile(FilePathHelper.GENERIC_MATERIAL_PATH).First().Item2;
        }

        public override void SetUniforms(ShaderProgram program)
        {
            _modelMatrix.Set(program);
            Material.SetUniforms(program);
        }

        public override bool CompareUniforms(IEntity entity) => base.CompareUniforms(entity) && entity is ShapeEntity;

        public override IRenderable ToRenderable()
        {
            var meshBuild = new MeshBuild(Shape);
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

            var vertices = meshVertices.Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)).ToList();
            var triangleIndices = meshBuild.TriangleIndices;
            return new Mesh<Vertex3D>(vertices, triangleIndices);
            //Vertices.AddRange(meshBuild.GetVertices().Select(v => new Vertex3D(v.Position, v.Normal, v.Tangent, v.UV)));
            //TriangleIndices.AddRange(meshBuild.TriangleIndices);
        }
    }
}
