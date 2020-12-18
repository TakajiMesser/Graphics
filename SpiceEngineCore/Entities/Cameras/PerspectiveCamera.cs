using SpiceEngineCore.Geometry.Vectors;
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

        public PerspectiveCamera(string name, float zNear, float zFar, float fieldOfViewY) : base(name, ProjectionTypes.Perspective)
        {
            _projectionMatrix.UpdatePerspective(fieldOfViewY, zNear, zFar);
        }

        public void CalculateTranslation()
        {
            var horizontal = _distance * Math.Cos(CurrentAngles.Y);
            var vertical = _distance * Math.Sin(CurrentAngles.Y);

            var x = -(float)(horizontal * Math.Sin(CurrentAngles.X));
            var y = -(float)(horizontal * Math.Cos(CurrentAngles.X));
            var z = (float)vertical;

            AttachedTranslation = new Vector3(x, y, z);
        }

        public void CalculateUp()
        {
            var yAngle = CurrentAngles.Y - MathExtensions.HALF_PI;

            var horizontal = _distance * Math.Cos(yAngle);
            var vertical = _distance * Math.Sin(yAngle);

            var x = -(float)(horizontal * Math.Sin(CurrentAngles.X));
            var y = -(float)(horizontal * Math.Cos(CurrentAngles.X));
            var z = (float)vertical;

            _viewMatrix.Up = new Vector3(x, y, z);
        }
    }
}