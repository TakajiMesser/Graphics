using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;

namespace SauceEditorCore.Models.Entities
{
    public abstract class ModelEntity<T> : Entity, IModelEntity where T : IModelShape
    {
        public T ModelShape { get; set; }

        public ModelEntity(T modelShape)
        {
            ModelShape = modelShape;
            base.Position = ModelShape.GetAveragePosition();
            ModelShape.CenterAround(Position);
            Transformed += (s, args) =>
            {
                ModelShape.Transform(args.Transform);
            };
        }

        public override void SetUniforms(ShaderProgram program)
        {
            program.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
            program.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
        }

        //public override bool CompareUniforms(IEntity entity) => entity is ModelEntity;// modelEntity && _modelMatrix.Equals(modelEntity._modelMatrix);

        public abstract IRenderable ToRenderable();
    }
}
