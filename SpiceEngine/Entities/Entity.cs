﻿using OpenTK;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Shaders;
using System;

namespace SpiceEngine.Entities
{
    public abstract class Entity : IEntity
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

        //public virtual void SetUniforms(ShaderProgram program) => _modelMatrix.Set(program);
        public abstract void SetUniforms(ShaderProgram program);

        public virtual bool CompareUniforms(IEntity entity) => entity is Entity castEntity && _modelMatrix.Equals(castEntity._modelMatrix);
    }
}
