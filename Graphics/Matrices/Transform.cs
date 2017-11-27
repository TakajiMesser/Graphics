using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.TwoDimensional;

namespace Graphics
{
    public class Transform
    {
        public Vector3 Translation { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Transform() { }
        public Transform(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        public Matrix4 ToModelMatrix()
        {
            var translationMatrix = Matrix4.CreateTranslation(Translation);
            var scaleMatrix = Matrix4.CreateScale(Scale);
            var rotationMatrix = Matrix4.CreateFromQuaternion(Rotation);

            return translationMatrix * rotationMatrix * scaleMatrix;
        }

        public void Set(ShaderProgram program)
        {
            //int location = program.GetUniformLocation(_name);
            //GL.UniformMatrix4(location, false, ref _matrix);
        }
    }
}
