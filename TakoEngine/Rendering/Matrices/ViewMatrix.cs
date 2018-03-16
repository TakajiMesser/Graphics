using OpenTK;
using TakoEngine.Rendering.Shaders;

namespace TakoEngine.Rendering.Matrices
{
    public class ViewMatrix
    {
        public const string NAME = "viewMatrix";
        public const string PREVIOUS_NAME = "previousViewMatrix";
        public const string SHADOW_NAME = "shadowViewMatrices";

        private Matrix4 _previousMatrix;

        internal Matrix4 Matrix
        {
            get
            {
                //var viewMatrix = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);

                var viewMatrix = Matrix4.LookAt(Translation, LookAt, Up);
                //viewMatrix.M41 = -Translation.X;
                //viewMatrix.M42 = -Translation.Y;
                //viewMatrix.M43 = -Translation.Z;

                return viewMatrix;
            }
        }

        public Vector3 Translation { get; set; } = Vector3.Zero;
        public Vector3 LookAt { get; set; } = -Vector3.UnitZ;
        public Vector3 Up { get; set; } = Vector3.UnitY;

        public ViewMatrix() { }

        public void Set(ShaderProgram program)
        {
            program.SetUniform(NAME, Matrix);
            program.SetUniform(PREVIOUS_NAME, _previousMatrix);

            _previousMatrix = Matrix;
        }
    }
}
