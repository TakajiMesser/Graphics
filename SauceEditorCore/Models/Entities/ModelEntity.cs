using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using System;

namespace SauceEditorCore.Models.Entities
{
    public abstract class ModelEntity : IModelEntity
    {
        protected ModelMatrix _modelMatrix = new ModelMatrix();

        public int ID { get; set; }

        public Vector3 Position
        {
            get => _modelMatrix.Translation;
            set
            {
                _modelMatrix.Translation = value;
                Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _modelMatrix.Matrix));
            }
        }

        public event EventHandler<EntityEventArgs> UniformsChanged;
        public event EventHandler<EntityTransformEventArgs> Transformed;

        public virtual void SetUniforms(ShaderProgram program)
        {
            program.SetUniform(ModelMatrix.NAME, Matrix4.Identity);
            program.SetUniform(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
        }

        public virtual bool CompareUniforms(IEntity entity) => entity is ModelEntity modelEntity && _modelMatrix.Equals(modelEntity._modelMatrix);

        public abstract IRenderable ToRenderable();
    }
}
