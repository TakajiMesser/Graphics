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
    public class ProjectionMatrix
    {
        public const string NAME = "projectionMatrix";

        //private Matrix4 _projection = Matrix4.Identity;
        internal Matrix4 Projection => Matrix4.CreateOrthographic(Width, Width / AspectRatio, ZNear, ZFar);

        public float Width { get; set; }
        public float AspectRatio { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public ProjectionMatrix() { }
        public ProjectionMatrix(float width, float aspectRatio, float zNear, float zFar)
        {
            Width = width;
            AspectRatio = aspectRatio;
            ZNear = zNear;
            ZFar = zFar;
        }

        public void Set(ShaderProgram program)
        {
            int location = program.GetUniformLocation(NAME);

            var projectionMatrix = Matrix4.CreateOrthographic(Width, Width / AspectRatio, ZNear, ZFar);
            GL.UniformMatrix4(location, false, ref projectionMatrix);
        }
    }
}
