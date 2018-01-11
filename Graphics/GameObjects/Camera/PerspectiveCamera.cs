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

namespace Graphics.GameObjects
{
    public class PerspectiveCamera : Camera
    {
        public const float ZNEAR = 0.1f;
        public const float ZFAR = 100.0f;

        private Vector3 _currentAngles = new Vector3(-(float)Math.PI, 0, 0);

        public PerspectiveCamera(string name, int width, int height, float fieldOfViewY) : base(name, width, height)
        {
            _projectionMatrix.Type = ProjectionTypes.Perspective;
            _projectionMatrix.FieldOfView = fieldOfViewY;
            _projectionMatrix.ZNear = ZNEAR;
            _projectionMatrix.ZFar = ZFAR;
        }

        public override void OnHandleInput(InputState inputState)
        {
            float amount = inputState.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                AttachedTranslation = new Vector3(AttachedTranslation.X, AttachedTranslation.Y, AttachedTranslation.Z - amount);
                _distanceFromPlayer = AttachedTranslation.Length;
            }

            if (inputState.IsHeld(new Input(MouseButton.Middle)))
            {
                var mouseDelta = inputState.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    var currentAngles = _currentAngles;
                    _currentAngles.X += mouseDelta.Y;
                    _currentAngles.Y += mouseDelta.X;

                    var horizontal = _distanceFromPlayer * Math.Cos(currentAngles.X);
                    var vertical = _distanceFromPlayer * Math.Sin(currentAngles.X + mouseDelta.Y);

                    AttachedTranslation = new Vector3()
                    {
                        X = (float)(horizontal * Math.Sin(currentAngles.Y + mouseDelta.X)),
                        Y = (float)vertical,
                        Z = (float)(horizontal * Math.Cos(currentAngles.Y + mouseDelta.X))
                    };
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
