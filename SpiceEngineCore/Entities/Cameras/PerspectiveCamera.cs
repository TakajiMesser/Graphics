using OpenTK;
using OpenTK.Input;
using SpiceEngineCore.Inputs;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;
using System;

namespace SpiceEngineCore.Entities.Cameras
{
    public class PerspectiveCamera : Camera
    {
        public const float ZNEAR = 0.1f;
        public const float ZFAR = 1000.0f;
        public const float MAX_ANGLE_Y = -MathExtensions.HALF_PI + 0.1f;
        public const float MIN_ANGLE_Y = -MathExtensions.PI + 0.1f;
        public const float MIN_DISTANCE = 1.0f;

        private Vector3 CurrentAngles { get; set; } = new Vector3(0, -MathExtensions.HALF_PI, 0);

        public PerspectiveCamera(string name, float zNear, float zFar, float fieldOfViewY) : base(name, ProjectionTypes.Perspective)
        {
            _projectionMatrix.UpdatePerspective(fieldOfViewY, zNear, zFar);
        }

        private void CalculateTranslation()
        {
            var horizontal = _distance * Math.Cos(CurrentAngles.Y);
            var vertical = _distance * Math.Sin(CurrentAngles.Y);

            AttachedTranslation = new Vector3()
            {
                X = -(float)(horizontal * Math.Sin(CurrentAngles.X)),
                Y = -(float)(horizontal * Math.Cos(CurrentAngles.X)),
                Z = (float)vertical
            };
        }

        private void CalculateUp()
        {
            var yAngle = CurrentAngles.Y - MathExtensions.HALF_PI;

            var horizontal = _distance * Math.Cos(yAngle);
            var vertical = _distance * Math.Sin(yAngle);

            _viewMatrix.Up = new Vector3()
            {
                X = -(float)(horizontal * Math.Sin(CurrentAngles.X)),
                Y = -(float)(horizontal * Math.Cos(CurrentAngles.X)),
                Z = (float)vertical
            };
        }

        public override void OnHandleInput(InputManager inputManager)
        {
            float amount = inputManager.MouseWheelDelta * 1.0f;
            if (amount > 0.0f || amount < 0.0f)
            {
                _distance += amount;
                if (_distance < MIN_DISTANCE)
                {
                    _distance = MIN_DISTANCE;
                }

                CalculateTranslation();
                CalculateUp();
            }

            var currentAngles = CurrentAngles;

            if (inputManager.IsDown(new Input(MouseButton.Right)))
            {
                var mouseDelta = inputManager.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    currentAngles.X += mouseDelta.X;
                    currentAngles.Y += mouseDelta.Y;
                    CurrentAngles = currentAngles;
                    //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                    CalculateTranslation();
                    CalculateUp();
                }
            }
            else
            {
                var cameraSpeed = 0.02f;

                if (inputManager.IsDown(new Input(Key.Up)))
                {
                    currentAngles.Y += cameraSpeed;
                }

                if (inputManager.IsDown(new Input(Key.Down)))
                {
                    currentAngles.Y -= cameraSpeed;
                }

                if (inputManager.IsDown(new Input(Key.Right)))
                {
                    currentAngles.X -= cameraSpeed;
                }

                if (inputManager.IsDown(new Input(Key.Left)))
                {
                    currentAngles.X += cameraSpeed;
                }

                CurrentAngles = currentAngles;
                //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                CalculateTranslation();
                CalculateUp();
            }
        }
    }
}