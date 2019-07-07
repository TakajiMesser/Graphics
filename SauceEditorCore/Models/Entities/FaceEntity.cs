using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class FaceEntity : ModelEntity
    {
        public MeshFace Face { get; }

        public FaceEntity(MeshFace meshFace) => Face = meshFace;

        public override bool CompareUniforms(IEntity entity) => base.CompareUniforms(entity) && entity is FaceEntity;

        public override IRenderable ToRenderable()
        {
            var meshBuild = new MeshBuild(Face);
            var meshVertices = meshBuild.GetVertices();

            var mesh = meshVertices.Any(v => v.IsAnimated)
                ? (IMesh)new Mesh<JointVertex3D>(meshBuild.GetVertices().Select(v => v.ToJointVertex3D()).ToList(), meshBuild.TriangleIndices)
                : new Mesh<Vertex3D>(meshBuild.GetVertices().Select(v => v.ToVertex3D()).ToList(), meshBuild.TriangleIndices);

            mesh.Transform(_modelMatrix.Matrix);
            return mesh;
        }
    }
}
