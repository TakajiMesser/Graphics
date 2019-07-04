using OpenTK;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Vertices;
using System.Linq;

namespace SauceEditorCore.Models.Entities
{
    public class TriangleEntity : IModelEntity
    {
        private ModelMatrix _modelMatrix = new ModelMatrix();

        public int ID { get; set; }
        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set => _modelMatrix.Translation = value;
        }
        public MeshTriangle Triangle { get; }

        public TriangleEntity(MeshTriangle meshTriangle) => Triangle = meshTriangle;

        public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);

        public IRenderable ToRenderable()
        {
            var meshBuild = new MeshBuild(Triangle);
            var meshVertices = meshBuild.GetVertices();

            if (meshVertices.Any(v => v.IsAnimated))
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToJointVertex3D());
                return new Mesh<JointVertex3D>(vertices.ToList(), meshBuild.TriangleIndices);
            }
            else
            {
                var vertices = meshBuild.GetVertices().Select(v => v.ToVertex3D());
                return new Mesh<Vertex3D>(vertices.ToList(), meshBuild.TriangleIndices);
            }
        }
    }
}
