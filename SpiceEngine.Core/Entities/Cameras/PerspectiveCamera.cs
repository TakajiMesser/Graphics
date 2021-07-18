using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngineCore.Entities.Cameras
{
    public class PerspectiveCamera : Camera
    {
        public const float ZNEAR = 0.1f;
        public const float ZFAR = 1000.0f;
        public const float MAX_ANGLE_Y = -MathExtensions.HALF_PI + 0.1f;
        public const float MIN_ANGLE_Y = -MathExtensions.PI + 0.1f;
        public const float MIN_DISTANCE = 1.0f;

        public float Distance
        {
            get => _distance;
            set
            {
                _distance = value;

                if (_distance < MIN_DISTANCE)
                {
                    _distance = MIN_DISTANCE;
                }
            }
        }

        public Vector3 CurrentAngles { get; set; } = new Vector3(0, -MathExtensions.HALF_PI, 0);

        public PerspectiveCamera(string name, float zNear, float zFar, float fieldOfViewY) : base(name, ProjectionTypes.Perspective) => _projectionMatrix.InitializePerspective(fieldOfViewY, zNear, zFar);

        public void CalculateTranslation()
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

        public void CalculateUp()
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
    }
}