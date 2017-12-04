using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    public class Camera : GameObject
    {
        private Matrix4Uniform _viewMatrix;
        private Matrix4Uniform _projectionMatrix;
        private ShaderProgram _program;

        public Camera(string name, ShaderProgram program, float width, float height) : base(name)
        {
            _program = program;

            _viewMatrix = new Matrix4Uniform("viewMatrix")
            {
                Matrix = Matrix4.Identity//Matrix4.LookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 3) + new Vector3(0, 0, -1.0f), new Vector3(0, 1, 0))
            };

            // Specify the FOV
            _projectionMatrix = new Matrix4Uniform("projectionMatrix")
            {
                Matrix = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 2), width / height, 1.0f, 100.0f)
            };
        }

        public override void OnRenderFrame()
        {
            _viewMatrix.Set(_program);
            _projectionMatrix.Set(_program);
        }
    }
}
