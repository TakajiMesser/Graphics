using OpenTK;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngineCore.Rendering.Matrices
{
    public class ViewMatrix
    {
        public const string CURRENT_NAME = "viewMatrix";
        public const string PREVIOUS_NAME = "previousViewMatrix";
        public const string SHADOW_NAME = "shadowViewMatrices";

        private Vector3 _translation = Vector3.Zero;
        private Vector3 _lookAt = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;

        public ViewMatrix() { }
        public ViewMatrix(Vector3 translation, Vector3 lookAt, Vector3 up) => Update(translation, lookAt, up);

        public Matrix4 CurrentValue { get; private set; }
        public Matrix4 PreviousValue { get; private set; }

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

        public void Update(Vector3 translation, Vector3 lookAt, Vector3 up)
        {
            _translation = translation;
            _lookAt = lookAt;
            _up = up;

            CalculateMatrix();
        }

        public void Set(ShaderProgram program)
        {
            program.SetUniform(CURRENT_NAME, CurrentValue);
            program.SetUniform(PREVIOUS_NAME, PreviousValue);

            PreviousValue = CurrentValue;
        }

        private void CalculateMatrix()
        {
            CurrentValue = Matrix4.LookAt(Translation, LookAt, Up);
        }
    }
}
