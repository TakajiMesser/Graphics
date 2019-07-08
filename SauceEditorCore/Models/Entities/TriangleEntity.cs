using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class TriangleEntity : ModelEntity
    {
        public MeshTriangle Triangle { get; }

        public TriangleEntity(MeshTriangle meshTriangle) => Triangle = meshTriangle;

        public override bool CompareUniforms(IEntity entity) => base.CompareUniforms(entity) && entity is TriangleEntity;

        public override IRenderable ToRenderable()
        {
            var meshBuild = new MeshBuild(Triangle);
            var meshVertices = meshBuild.GetVertices();

            var mesh = meshVertices.Any(v => v.IsAnimated)
                ? (IMesh)new Mesh<AnimatedVertex3D>(meshBuild.GetVertices().Select(v => v.ToJointVertex3D()).ToList(), meshBuild.TriangleIndices)
                : new Mesh<Vertex3D>(meshBuild.GetVertices().Select(v => v.ToVertex3D()).ToList(), meshBuild.TriangleIndices);

            mesh.Transform(_modelMatrix.Matrix);
            return mesh;
        }
    }
}
