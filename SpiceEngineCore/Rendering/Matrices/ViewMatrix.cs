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
    public class ViewMatrix
    {
        public const string CURRENT_NAME = "viewMatrix";
        public const string PREVIOUS_NAME = "previousViewMatrix";
        public const string SHADOW_NAME = "shadowViewMatrices";

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

        private Vector3 _translation = Vector3.Zero;
        private Vector3 _lookAt = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;

        public ViewMatrix() { }
        public ViewMatrix(Vector3 translation, Vector3 lookAt, Vector3 up) => Update(translation, lookAt, up);

        public void Update(Vector3 translation, Vector3 lookAt, Vector3 up)
        {
            _translation = translation;
            _lookAt = lookAt;
            _up = up;

            CalculateMatrix();
        }

        private void CalculateMatrix()
        {
            PreviousValue = CurrentValue;
            CurrentValue = Matrix4.LookAt(Translation, LookAt, Up);
        }
    }
}
