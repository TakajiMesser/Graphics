using OpenTK;

namespace SpiceEngineCore.Rendering.Matrices
{
    public class ViewMatrix : TransformMatrix
    {
        public const string CURRENT_NAME = "viewMatrix";
        public const string PREVIOUS_NAME = "previousViewMatrix";
        public const string SHADOW_NAME = "shadowViewMatrices";

        private Vector3 _translation = Vector3.Zero;
        private Vector3 _lookAt = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;

        public ViewMatrix() { }
        public ViewMatrix(Vector3 translation, Vector3 lookAt, Vector3 up)
        {
            _translation = translation;
            _lookAt = lookAt;
            _up = up;

            InitializeValue(Calculate());
        }

        public Vector3 Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                UpdateValue(Calculate());
            }
        }

        public Vector3 LookAt
        {
            get => _lookAt;
            set
            {
                _lookAt = value;
                UpdateValue(Calculate());
            }
        }

        public Vector3 Up
        {
            get => _up;
            set
            {
                _up = value;
                UpdateValue(Calculate());
            }
        }

        private Matrix4 Calculate() => Matrix4.LookAt(Translation, LookAt, Up);
    }
}
