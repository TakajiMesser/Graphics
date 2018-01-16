using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Shaders;
using Graphics.Outputs;

namespace Graphics.Rendering.Matrices
{
    public enum ProjectionTypes
    {
        Orthographic,
        Perspective
    }

    public class ProjectionMatrix
    {
        public const string NAME = "projectionMatrix";

        //private Matrix4 _projection = Matrix4.Identity;

        public ProjectionTypes Type { get; set; }
        public float Width { get; set; }
        public float FieldOfView { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }

        public Resolution Resolution { get; set; }

        public Matrix4 Projection
        {
            get
            {
                switch (Type)
                {
                    case ProjectionTypes.Orthographic:
                        return Matrix4.CreateOrthographic(Width, Width / Resolution.AspectRatio, ZNear, ZFar);
                    case ProjectionTypes.Perspective:
                        return Matrix4.CreatePerspectiveFieldOfView(FieldOfView, Resolution.AspectRatio, ZNear, ZFar);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public ProjectionMatrix() { }
        public ProjectionMatrix(float width, float zNear, float zFar)
        {
            Width = width;
            ZNear = zNear;
            ZFar = zFar;
        }

        public void Set(ShaderProgram program)
        {
            int location = program.GetUniformLocation(NAME);

            var projection = Projection;
            GL.UniformMatrix4(location, false, ref projection);
        }
    }
}
