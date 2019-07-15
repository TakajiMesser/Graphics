using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Utilities;
using System;

namespace SauceEditorCore.Models.Entities
{
    public abstract class ModelEntity : Entity, IModelEntity
    {
        public override void SetUniforms(ShaderProgram program)
        {
            program.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
            program.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
        }

        //public override bool CompareUniforms(IEntity entity) => entity is ModelEntity;// modelEntity && _modelMatrix.Equals(modelEntity._modelMatrix);

        public abstract IRenderable ToRenderable();
    }
}
