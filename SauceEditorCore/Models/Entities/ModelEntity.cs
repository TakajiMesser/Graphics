using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Shaders;

namespace SauceEditorCore.Models.Entities
{
    public abstract class ModelEntity<T> : Entity, IEntityBuilder, IRenderableBuilder where T : IModelShape
    {
        public T ModelShape { get; set; }

        //public abstract Vector3 Rotation { get; set; }
        //public abstract Vector3 Scale { get; set; }

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
        public virtual IEntity ToEntity() => this;
        public abstract IRenderable ToRenderable();
    }
}
