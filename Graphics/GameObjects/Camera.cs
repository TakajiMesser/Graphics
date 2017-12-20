using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Rendering.Shaders;

namespace Graphics.GameObjects
{
    public class Camera
    {
        public const float ZNEAR = -1.0f;
        public const float ZFAR = 1.0f;

        private string _name;
        private ShaderProgram _program;
        public Matrix4Uniform _viewMatrix;
        public Matrix4Uniform _projectionMatrix;

        private float _aspectRatio;
        private float _width = 20.0f;
        //private Matrix4 _zoomScale = Matrix4.Identity;
        //private float _scaleAmount = 1.0f;

        public GameObject AttachedObject { get; set; }
        public Transform Transform { get; set; }

        public Camera(string name, ShaderProgram program, int width, int height)
        {
            _name = name;
            _program = program;
            _aspectRatio = (float)width / height;

            _viewMatrix = new Matrix4Uniform("viewMatrix")
            {
                Matrix = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY)
            };

            _projectionMatrix = new Matrix4Uniform("projectionMatrix")
            {
                Matrix = Matrix4.CreateOrthographic(_width, _width / _aspectRatio, ZNEAR, ZFAR)
            };
        }

        public void UpdateAspectRatio(int width, int height)
        {
            _aspectRatio = (float)width / height;
            _projectionMatrix.Matrix = Matrix4.CreateOrthographic(_width, _width / _aspectRatio, ZNEAR, ZFAR);
        }

        public void OnUpdateFrame()
        {
            if (AttachedObject != null)
            {
                var viewMatrix = _viewMatrix.Matrix;
                viewMatrix.M41 = -AttachedObject.Position.X;
                viewMatrix.M42 = -AttachedObject.Position.Y;

                _viewMatrix.Matrix = viewMatrix;
            }
        }

        public void OnHandleInput(KeyboardState keyState, MouseState mouseState, KeyboardState previousKeyState, MouseState previousMouseState)
        {
            float amount = (previousMouseState.Wheel - mouseState.Wheel) * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _width += amount;
                _projectionMatrix.Matrix = Matrix4.CreateOrthographic(_width, _width / _aspectRatio, ZNEAR, ZFAR);
            }
        }

        public void OnRenderFrame()
        {
            _viewMatrix.Set(_program);
            _projectionMatrix.Set(_program);
        }
    }
}
