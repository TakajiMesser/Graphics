using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Utilities;
using System;

namespace SauceEditorCore.Models.Entities
{
    public abstract class ModelEntity<T> : Entity, IModelEntity where T : IModelShape
    {
        public T ModelShape { get; set; }

        public override Vector3 Position
        {
            get => base.Position;
            set
            {
                var translation = value - Position;
                if (translation.IsSignificant())
                {
                    ModelShape.Translate(translation.X, translation.Y, translation.Z);
                }

                base.Position = value;
            }
        }

        public ModelEntity(T modelShape)
        {
            ModelShape = modelShape;
            base.Position = ModelShape.GetAveragePosition();
            ModelShape.CenterAround(Position);
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
