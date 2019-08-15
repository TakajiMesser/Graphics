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

        public Vector3 Position
        {
            get => _modelMatrix.Position;
            set => _modelMatrix.Position = value;
        }

        public virtual void Transform(Transform transform) => _modelMatrix.Transform(transform);

        private event EventHandler<EntityTransformEventArgs> _transformed;

        public event EventHandler<EntityEventArgs> UniformsChanged;
        public event EventHandler<EntityTransformEventArgs> Transformed
        {
            add
            {
                if (_transformed == null)
                {
                    _modelMatrix.Transformed += OnTransformed;
                }

                _transformed += value;
            }
            remove
            {
                _transformed -= value;

                if (_transformed == null)
                {
                    _modelMatrix.Transformed -= OnTransformed;
                }
            }
        }

        //public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);
        public abstract void SetUniforms(ShaderProgram program);

        public virtual bool CompareUniforms(IEntity entity) => entity is Entity castEntity && _modelMatrix.Equals(castEntity._modelMatrix);

        private virtual void OnTransformed(object sender, TransformEventArgs e) => Transformed?.Invoke(this, new EntityTransformEventArgs(ID, e.Transform));
    }
}
