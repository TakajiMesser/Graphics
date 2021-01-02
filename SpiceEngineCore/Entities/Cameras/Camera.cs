using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Matrices;
using System;

namespace SpiceEngineCore.Entities.Cameras
{
    public abstract class Camera : ICamera
    {
        private bool _isActive;

        protected ViewMatrix _viewMatrix = new ViewMatrix();
        protected ProjectionMatrix _projectionMatrix;
        protected float _distance;

        public Camera(string name, ProjectionTypes projectionType)
        {
            Name = name;
            _projectionMatrix = new ProjectionMatrix(projectionType);
        }

        public int ID { get; set; }
        public string Name { get; }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (!_isActive && value)
                {
                    BecameActive?.Invoke(this, EventArgs.Empty);
                }

                _isActive = value;
            }
        }

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
        
        // While a bit janky, we are outwardly referring to the camera's ViewMatrix as its ModelMatrix
        public Matrix4 CurrentModelMatrix => _viewMatrix.CurrentValue;
        public Matrix4 PreviousModelMatrix => _viewMatrix.CurrentValue;
        public Matrix4 CurrentProjectionMatrix => _projectionMatrix.CurrentValue;
        public Matrix4 PreviousProjectionMatrix => _projectionMatrix.CurrentValue;

        public event EventHandler BecameActive;
        public event EventHandler<EntityTransformEventArgs> Transformed;

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
