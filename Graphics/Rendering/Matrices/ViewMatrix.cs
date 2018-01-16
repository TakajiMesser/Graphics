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
    public class ViewMatrix
    {
        public const string NAME = "viewMatrix";

        //private Matrix4 _model = Matrix4.Identity;
        internal Matrix4 View
        {
            get
            {
                //var viewMatrix = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);

                var viewMatrix = Matrix4.LookAt(Translation, Translation - Vector3.UnitZ, Vector3.UnitY);
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
            int location = program.GetUniformLocation(NAME);

            var viewMatrix = Matrix4.LookAt(Translation, LookAt, Up);
            /*viewMatrix.M41 = -Translation.X;
            viewMatrix.M42 = -Translation.Y;
            viewMatrix.M43 = -Translation.Z;*/

            GL.UniformMatrix4(location, false, ref viewMatrix);
        }
    }
}
