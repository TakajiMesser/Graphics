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

namespace SpiceEngineCore.Entities.Cameras
{
    public abstract class Camera : ICamera
    {
        public int ID { get; set; }
        public string Name { get; }
        public bool IsActive { get; set; }

        // TODO - Fix this...
        public Matrix4 ModelMatrix => throw new NotImplementedException();
        public Matrix4 PreviousModelMatrix => throw new NotImplementedException();
        public ModelMatrix WorldMatrix => throw new NotImplementedException();

        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set
            {
                _viewMatrix.Translation = value;
                //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _viewMatrix.Matrix));
            }
        }

        public Vector3 LookAt
        {
            get => _viewMatrix.LookAt;
            set => _viewMatrix.LookAt = value;
        }

        public Vector3 Up
        {
            get => _viewMatrix.Up;
            set => _viewMatrix.Up = value;
        }

        public IEntity AttachedEntity { get; private set; }
        public Vector3 AttachedTranslation { get; protected set; }

        public Matrix4 ViewMatrix => _viewMatrix.CurrentValue;
        public Matrix4 PreviousViewMatrix => _viewMatrix.PreviousValue;
        public Matrix4 ProjectionMatrix => _projectionMatrix.CurrentValue;
        public Matrix4 PreviousProjectionMatrix => _projectionMatrix.PreviousValue;

        protected ViewMatrix _viewMatrix = new ViewMatrix();
        protected ProjectionMatrix _projectionMatrix;
        protected float _distance;

        public event EventHandler<EntityTransformEventArgs> Transformed;

        public Camera(string name, ProjectionTypes projectionType)
        {
            Name = name;
            _projectionMatrix = new ProjectionMatrix(projectionType);
        }

        public void Transform(Transform transform) => throw new NotImplementedException();

        public void Translate(Vector3 translation)
        {
            _viewMatrix.Translation *= translation;
            //Transformed?.Invoke(this, new EntityTransformEventArgs(ID, _viewMatrix.Matrix));
        }

        public void UpdateAspectRatio(float value) => _projectionMatrix.AspectRatio = value;

        public void AttachToEntity(IEntity entity, bool attachTranslation, bool attachRotation)
        {
            AttachedEntity = entity;

            // Determine the original distance from the attached object, based on the current camera position
            AttachedTranslation = entity.Position - Position;
            _distance = AttachedTranslation.Length;

            AttachedEntity.Transformed += (s, args) =>
            {
                Position = args.Position - AttachedTranslation;
                LookAt = AttachedEntity.Position;
            };
        }

        public void DetachFromEntity()
        {
            AttachedEntity = null;
            AttachedTranslation = Vector3.Zero;
            _distance = 0.0f;
        }
    }
}
