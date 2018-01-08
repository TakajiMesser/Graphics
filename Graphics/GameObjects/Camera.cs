using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics.Rendering.Shaders;
using Graphics.Inputs;
using Graphics.Rendering.Matrices;

namespace Graphics.GameObjects
{
    public class Camera
    {
        public const float ZNEAR = -10.0f;
        public const float ZFAR = 10.0f;

        private string _name;
        private ViewMatrix _viewMatrix = new ViewMatrix();
        private ProjectionMatrix _projectionMatrix = new ProjectionMatrix();
        private float _width = 20.0f;

        public GameObject AttachedObject { get; set; }
        public Vector3 Position
        {
            get => _viewMatrix.Translation;
            set => _viewMatrix.Translation = value;
        }
        public Matrix4 ViewProjectionMatrix => _projectionMatrix.Projection * _viewMatrix.View;

        public Camera(string name, int width, int height)
        {
            _name = name;
            _projectionMatrix.Width = _width;
            _projectionMatrix.AspectRatio = (float)width / height;
            _projectionMatrix.ZNear = ZNEAR;
            _projectionMatrix.ZFar = ZFAR;
        }

        public void UpdateAspectRatio(int width, int height)
        {
            _projectionMatrix.AspectRatio = (float)width / height;
        }

        public void OnUpdateFrame()
        {
            if (AttachedObject != null)
            {
                Position = AttachedObject.Position;
            }
        }

        public void OnHandleInput(InputState inputState)
        {
            float amount = inputState.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _width += amount;
                _projectionMatrix.Width = _width;
            }
        }

        public void Draw(ShaderProgram program)
        {
            _viewMatrix.Set(program);
            _projectionMatrix.Set(program);
        }
    }
}
