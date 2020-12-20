using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using System;

namespace SpiceEngineCore.Entities
{
    public abstract class Entity : IEntity
    {
        protected ModelMatrix _modelMatrix = new ModelMatrix();

        public int ID { get; set; }

        public virtual Vector3 Position
        {
            get => _modelMatrix.Position;
            set => _modelMatrix.Position = value;
        }

        public ModelMatrix WorldMatrix => _modelMatrix;

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
                OnTransformed(this, new TransformEventArgs(_modelMatrix.WorldTransform));
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

        public virtual void Transform(Transform transform) => _modelMatrix.Transform(transform);

        // TODO - By the time we've hooked up the transform event, it could be too late, so we need to fire it as soon as we hook up
        private void OnTransformed(object sender, TransformEventArgs e) => _transformed?.Invoke(this, new EntityTransformEventArgs(ID, Position, e.Transform));
    }
}
