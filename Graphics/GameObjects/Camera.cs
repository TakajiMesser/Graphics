using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.GameObjects
{
    public class Camera
    {
        public const float ZNEAR = -1.0f;
        public const float ZFAR = 1.0f;

        private string _name;
        private ShaderProgram _program;
        private Matrix4Uniform _viewMatrix;
        private Matrix4Uniform _projectionMatrix;
        private GameObject _attachedObject;

        private float _aspectRatio;
        private int _width = 20;

        public Matrix4Uniform View => _viewMatrix;
        public Matrix4Uniform Projection => _projectionMatrix;
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
            /*if (Transform != null)
            {
                _viewMatrix.Matrix *= Matrix4.CreateTranslation(Transform.Translation);
            }*/
            
            if (_attachedObject != null)
            {
                var viewMatrix = _viewMatrix.Matrix.ClearTranslation();
                viewMatrix.M41 = -_attachedObject.Position.X;
                viewMatrix.M42 = -_attachedObject.Position.Y;

                _viewMatrix.Matrix = viewMatrix;
                //_viewMatrix.Matrix *= Transform.FromTranslation(new Vector3(-_attachedObject.Position.X, -_attachedObject.Position.Y, 0.0f)).ToModelMatrix();
            }
        }

        public void OnHandleInput(KeyboardState keyState, MouseState mouseState, KeyboardState previousKeyState, MouseState previousMouseState)
        {
            float amount = (previousMouseState.Wheel - mouseState.Wheel) * 1.0f;
            _width += (int)amount;

            _projectionMatrix.Matrix = Matrix4.CreateOrthographic(_width, _width / _aspectRatio, ZNEAR, ZFAR);
        }

        public void AttachToObject(GameObject gameObject)
        {
            _attachedObject = gameObject;
        }

        public void OnRenderFrame()
        {
            _viewMatrix.Set(_program);
            _projectionMatrix.Set(_program);
        }
    }
}
