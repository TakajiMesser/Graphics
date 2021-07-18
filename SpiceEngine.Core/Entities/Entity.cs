using SpiceEngineCore.Rendering.Matrices;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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

        public Matrix4 CurrentModelMatrix => _modelMatrix.CurrentValue;
        public Matrix4 PreviousModelMatrix => _modelMatrix.PreviousValue;

        private event EventHandler<EntityTransformEventArgs> _transformed;

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
