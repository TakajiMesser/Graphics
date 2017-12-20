using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.Geometry.TwoDimensional;
using Graphics.Rendering.Shaders;

namespace Graphics
{
    public class Transform
    {
        public Vector3 Translation { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

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
            //return rotationMatrix * translationMatrix * scaleMatrix;
        }

        public void Set(ShaderProgram program)
        {
            //int location = program.GetUniformLocation(_name);
            //GL.UniformMatrix4(location, false, ref _matrix);
        }

        public void PerformRotationInPlace(Quaternion rotation)
        {

        }

        public static Transform FromTranslation(Vector3 translation)
        {
            return new Transform()
            {
                Translation = translation
            };
        }

        public static Transform FromRotation(Quaternion rotation)
        {
            return new Transform()
            {
                Rotation = rotation
            };
        }

        public static Transform FromScale(Vector3 scale)
        {
            return new Transform()
            {
                Scale = scale
            };
        }
    }
}
