using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Shaders;

namespace Graphics.Rendering.Matrices
{
    public class ModelMatrix
    {
        public const string NAME = "modelMatrix";
        public const string PREVIOUS_NAME = "previousModelMatrix";

        private Matrix4 _previousMatrix;
        //private Matrix4 _model = Matrix4.Identity;

        public Matrix4 Matrix => Matrix4.Identity * Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Translation);

        public Vector3 Translation { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public ModelMatrix() { }
        public ModelMatrix(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        public void Set(ShaderProgram program)
        {
            program.SetUniformMatrix(NAME, Matrix);
            program.SetUniformMatrix(PREVIOUS_NAME, _previousMatrix);

            _previousMatrix = Matrix;
        }
    }
}
