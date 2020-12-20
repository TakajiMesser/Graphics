using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
