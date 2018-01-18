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
using Graphics.Utilities;
using Graphics.Outputs;

namespace Graphics.GameObjects
{
    public class PerspectiveCamera : Camera
    {
        public const float ZNEAR = 0.1f;
        public const float ZFAR = 100.0f;
        public const float MAX_ANGLE_Y = -(float)Math.PI / 2 + 0.1f;
        public const float MIN_ANGLE_Y = -(float)Math.PI + 0.1f;

        private Vector3 _currentAngles = new Vector3(0, -(float)Math.PI / 2.0f, 0);

        public PerspectiveCamera(string name, Resolution resolution, float fieldOfViewY) : base(name, resolution)
        {
            _projectionMatrix.Type = ProjectionTypes.Perspective;
            _projectionMatrix.FieldOfView = fieldOfViewY;
            _projectionMatrix.ZNear = ZNEAR;
            _projectionMatrix.ZFar = ZFAR;
        }

        private void CalculateTranslation()
        {
            var horizontal = _distance * Math.Cos(_currentAngles.Y);
            var vertical = _distance * Math.Sin(_currentAngles.Y);

            AttachedTranslation = new Vector3()
            {
                X = -(float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = -(float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = (float)vertical
            };
        }

        private void CalculateUp()
        {
            var yAngle = _currentAngles.Y - (float)Math.PI / 2.0f;

            var horizontal = _distance * Math.Cos(yAngle);
            var vertical = _distance * Math.Sin(yAngle);

            _viewMatrix.Up = new Vector3()
            {
                X = -(float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = -(float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = (float)vertical
            };
        }

        public override void OnHandleInput(InputState inputState)
        {
            float amount = inputState.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _distance += amount;
                CalculateTranslation();
                CalculateUp();
            }

            if (inputState.IsHeld(new Input(MouseButton.Middle)))
            {
                var mouseDelta = inputState.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    _currentAngles.X += mouseDelta.X;
                    _currentAngles.Y += mouseDelta.Y;
                    _currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                    CalculateTranslation();
                    CalculateUp();
                }
            }
        }

        private Vector3 GetCurrentAngles()
        {
            return new Vector3()
            {
                X = (float)Math.Acos(Vector2.Dot(_viewMatrix.Translation.Yz, _viewMatrix.LookAt.Yz)),
                Y = (float)Math.Acos(Vector2.Dot(_viewMatrix.Translation.Xz, _viewMatrix.LookAt.Xz)),
                Z = (float)Math.Acos(Vector2.Dot(_viewMatrix.Translation.Xy, _viewMatrix.LookAt.Xy))
            };
        }
    }
}
