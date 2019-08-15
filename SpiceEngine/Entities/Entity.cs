using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Utilities;
using System;

namespace SpiceEngine.Entities
{
    public abstract class Entity : IEntity
    {
        protected ModelMatrix _modelMatrix = new ModelMatrix();

        public int ID { get; set; }

        public virtual Vector3 Position
        {
            get => _modelMatrix.Translation;
            set
            {
                var displacement = value - _modelMatrix.Translation;
                _modelMatrix.Translation = value;

                if (displacement.IsSignificant())
                {
                    OnTransformed(this, new EntityTransformEventArgs(ID, Matrix4.CreateTranslation(displacement)));
                }
            }
        }

        public event EventHandler<EntityEventArgs> UniformsChanged;
        public event EventHandler<EntityTransformEventArgs> Transformed;

        public void Translate(Vector3 translation)
        {
            _modelMatrix.Translation += translation;
            OnTransformed(this, new EntityTransformEventArgs(ID, Matrix4.CreateTranslation(translation)));
        }

        public Matrix4 GetModelMatrix() => _modelMatrix.Matrix;

        //public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);
        public abstract void SetUniforms(ShaderProgram program);

        public virtual bool CompareUniforms(IEntity entity) => entity is Entity castEntity && _modelMatrix.Equals(castEntity._modelMatrix);

        protected virtual void OnTransformed(object sender, EntityTransformEventArgs e) => Transformed?.Invoke(sender, e);
    }
}
