﻿using SpiceEngineCore.Rendering.Matrices;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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

        public Matrix4 ModelMatrix => _modelMatrix.CurrentValue;
        public Matrix4 PreviousModelMatrix => _modelMatrix.PreviousValue;
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

        //public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);
        //public abstract void SetUniforms(ShaderProgram program);

        //public virtual bool CompareUniforms(IEntity entity) => entity is Entity castEntity && _modelMatrix.Equals(castEntity._modelMatrix);

        // TODO - By the time we've hooked up the transform event, it could be too late, so we need to fire it as soon as we hook up
        private void OnTransformed(object sender, TransformEventArgs e) => _transformed?.Invoke(this, new EntityTransformEventArgs(ID, Position, e.Transform));
    }
}
