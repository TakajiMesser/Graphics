using OpenTK;
using SpiceEngine.Rendering.Shaders;

namespace SpiceEngine.Rendering.Matrices
{
    public class ModelMatrix
    {
        public const string NAME = "modelMatrix";
        public const string PREVIOUS_NAME = "previousModelMatrix";

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

        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                CalculateMatrix();
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                CalculateMatrix();
            }
        }

        private Vector3 _translation = Vector3.Zero;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3 _scale = Vector3.One;

        private Matrix4 _previousMatrix;

        public ModelMatrix() { }
        public ModelMatrix(Vector3 translation, Quaternion rotation, Vector3 scale) => Update(translation, rotation, scale);

        public void Update(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            _translation = translation;
            _rotation = rotation;
            _scale = scale;

            CalculateMatrix();
        }

        public void Set(ShaderProgram program)
        {
            program.SetUniform(NAME, Matrix);
            program.SetUniform(PREVIOUS_NAME, _previousMatrix);

            _previousMatrix = Matrix;
        }

        private void CalculateMatrix() =>
            Matrix = Matrix4.Identity * Matrix4.CreateScale(_scale) * Matrix4.CreateFromQuaternion(_rotation) * Matrix4.CreateTranslation(_translation);
    }
}
