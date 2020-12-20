using OpenTK;
using System;

namespace SpiceEngineCore.Rendering.Matrices
{
    public enum ProjectionTypes
    {
        Orthographic,
        Perspective
    }

    public class ProjectionMatrix : TransformMatrix
    {
        public const string CURRENT_NAME = "projectionMatrix";
        public const string PREVIOUS_NAME = "previousProjectionMatrix";

        private float _width = 0.0f;
        private float _aspectRatio = 1.0f;
        private float _fieldOfView = 0.0f;
        private float _zNear = 0.0f;
        private float _zFar = 0.0f;

        public ProjectionMatrix(ProjectionTypes type) => Type = type;

        public ProjectionTypes Type { get; }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                UpdateValue(Calculate());
            }
        }

        public float AspectRatio
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                UpdateValue(Calculate());
            }
        }

        public float FieldOfView
        {
            get => _fieldOfView;
            set
            {
                _fieldOfView = value;
                UpdateValue(Calculate());
            }
        }

        public float ZNear
        {
            get => _zNear;
            set
            {
                _zNear = value;
                UpdateValue(Calculate());
            }
        }

        public float ZFar
        {
            get => _zFar;
            set
            {
                _zFar = value;
                UpdateValue(Calculate());
            }
        }

        public void InitializeOrthographic(float width, float zNear, float zFar)
        {
            _width = width;
            _zNear = zNear;
            _zFar = zFar;

            InitializeValue(Calculate());
        }

        public void InitializePerspective(float fieldOfView, float zNear, float zFar)
        {
            _fieldOfView = fieldOfView;
            _zNear = zNear;
            _zFar = zFar;

            InitializeValue(Calculate());
        }

        private Matrix4 Calculate()
        {
            switch (Type)
            {
                case ProjectionTypes.Orthographic:
                    return Matrix4.CreateOrthographic(_width, _width / _aspectRatio, _zNear, _zFar);
                case ProjectionTypes.Perspective:
                    return Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _zNear, _zFar);
            }

            throw new NotImplementedException("Could not handle projection matrix type " + Type);
        }
    }
}
