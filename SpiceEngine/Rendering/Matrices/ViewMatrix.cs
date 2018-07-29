using OpenTK;
using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Rendering.Matrices
{
    public class ViewMatrix
    {
        public const string NAME = "viewMatrix";
        public const string PREVIOUS_NAME = "previousViewMatrix";
        public const string SHADOW_NAME = "shadowViewMatrices";

        public Matrix4 Matrix { get; private set; }

        public Vector3 Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                CalculateMatrix();
            }
        }

        public Vector3 LookAt
        {
            get => _lookAt;
            set
            {
                _lookAt = value;
                CalculateMatrix();
            }
        }

        public Vector3 Up
        {
            get => _up;
            set
            {
                _up = value;
                CalculateMatrix();
            }
        }

        private Vector3 _translation = Vector3.Zero;
        private Vector3 _lookAt = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;

        private Matrix4 _previousMatrix;

        public ViewMatrix() { }
        public ViewMatrix(Vector3 translation, Vector3 lookAt, Vector3 up) => Update(translation, lookAt, up);

        public void Update(Vector3 translation, Vector3 lookAt, Vector3 up)
        {
            _translation = translation;
            _lookAt = lookAt;
            _up = up;

            CalculateMatrix();
        }

        public void Set(ShaderProgram program)
        {
            program.SetUniform(NAME, Matrix);
            program.SetUniform(PREVIOUS_NAME, _previousMatrix);

            _previousMatrix = Matrix;
        }

        private void CalculateMatrix() => Matrix = Matrix4.LookAt(Translation, LookAt, Up);
    }
}
