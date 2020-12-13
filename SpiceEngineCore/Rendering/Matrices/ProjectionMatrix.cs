using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Rendering.Matrices
{
    public enum ProjectionTypes
    {
        Orthographic,
        Perspective
    }

    public class ProjectionMatrix
    {
        public const string CURRENT_NAME = "projectionMatrix";
        public const string PREVIOUS_NAME = "previousProjectionMatrix";

        public Matrix4 CurrentValue { get; private set; }
        public Matrix4 PreviousValue { get; private set; }

        public ProjectionTypes Type { get; }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                CalculateMatrix();
            }
        }

        public float AspectRatio
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                CalculateMatrix();
            }
        }

        public float FieldOfView
        {
            get => _fieldOfView;
            set
            {
                _fieldOfView = value;
                CalculateMatrix();
            }
        }

        public float ZNear
        {
            get => _zNear;
            set
            {
                _zNear = value;
                CalculateMatrix();
            }
        }

        public float ZFar
        {
            get => _zFar;
            set
            {
                _zFar = value;
                CalculateMatrix();
            }
        }

        private float _width = 0.0f;
        private float _aspectRatio = 1.0f;
        private float _fieldOfView = 0.0f;
        private float _zNear = 0.0f;
        private float _zFar = 0.0f;

        public ProjectionMatrix(ProjectionTypes type) => Type = type;

        public void UpdateOrthographic(float width, float zNear, float zFar)
        {
            _width = width;
            _zNear = zNear;
            _zFar = zFar;

            CalculateMatrix();
        }

        public void UpdatePerspective(float fieldOfView, float zNear, float zFar)
        {
            _fieldOfView = fieldOfView;
            _zNear = zNear;
            _zFar = zFar;

            CalculateMatrix();
        }

        private void CalculateMatrix()
        {
            PreviousValue = CurrentValue;

            switch (Type)
            {
                case ProjectionTypes.Orthographic:
                    CurrentValue = Matrix4.CreateOrthographic(_width, _width / _aspectRatio, _zNear, _zFar);
                    break;
                case ProjectionTypes.Perspective:
                    CurrentValue = Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _zNear, _zFar);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
