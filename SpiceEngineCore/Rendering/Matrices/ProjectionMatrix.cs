using OpenTK;
using SpiceEngineCore.Rendering.Shaders;
using System;

namespace SpiceEngineCore.Rendering.Matrices
{
    public enum ProjectionTypes
    {
        Orthographic,
        Perspective
    }

    public class ProjectionMatrix
    {
        public const string NAME = "projectionMatrix";
        public const string PREVIOUS_NAME = "previousProjectionMatrix";

        public Matrix4 Matrix { get; private set; }

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

        private Matrix4 _previousMatrix;

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

        public void Set(ShaderProgram program)
        {
            program.SetUniform(NAME, Matrix);
            program.SetUniform(PREVIOUS_NAME, _previousMatrix);

            _previousMatrix = Matrix;
        }

        private void CalculateMatrix()
        {
            switch (Type)
            {
                case ProjectionTypes.Orthographic:
                    Matrix = Matrix4.CreateOrthographic(_width, _width / _aspectRatio, _zNear, _zFar);
                    break;
                case ProjectionTypes.Perspective:
                    Matrix = Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _zNear, _zFar);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
