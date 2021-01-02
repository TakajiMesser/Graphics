using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Renderers;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngine.Game
{
    public enum ViewTypes
    {
        X,
        Y,
        Z, 
        Perspective
    }

    public class PanelCamera
    {
        public const float MIN_DISTANCE = 1.0f;

        public const float MIN_PITCH = -MathExtensions.HALF_PI + 0.01f;
        public const float MAX_PITCH = MathExtensions.HALF_PI + 0.01f;

        // Math.Atan2(), which is used to calculate YAW, has a range of -pi to +pi
        // When we calculate YAW, we then add pi, resulting in a range of 0 to 2pi
        public const float MAX_YAW = MathExtensions.TWO_PI;

        private Resolution _resolution;
        private IEntityProvider _entityProvider;
        private IGridRenderer _gridRenderer;

        // Yaw of zero should point in the direction of the X-Axis
        private float _yaw;

        // Pitch of zero should point in the direction of the X-Axis
        private float _pitch;

        public Camera Camera { get; private set; }
        public ViewTypes ViewType { get; set; }

        public PanelCamera(Resolution resolution, IEntityProvider entityProvider, IGridRenderer gridRenderer)
        {
            _resolution = resolution;
            _entityProvider = entityProvider;
            _gridRenderer = gridRenderer;
        }

        public void CenterView(Vector3 position)
        {
            switch (ViewType)
            {
                case ViewTypes.Perspective:
                    // Determine new yaw and pitch
                    var lookDirection = (position - Camera.Position).Normalized();
                    _pitch = (float)Math.Asin(lookDirection.Z);
                    _yaw = ((float)Math.Atan2(lookDirection.Y, lookDirection.X) + MAX_YAW) % MAX_YAW;

                    /*if (mouseWheelDelta != 0.0f)
                    {
                        var translation = lookDirection * mouseWheelDelta * 1.0f;
                        Camera.Position -= translation;
                    }*/

                    CalculateTranslation(position);
                    CalculateUp();
                    break;
                case ViewTypes.X:
                    var translationX = new Vector3()
                    {
                        X = 0.0f,
                        Y = position.Y - Camera.Position.Y,
                        Z = position.Z - Camera.Position.Z
                    };

                    Camera.Position += translationX;
                    Camera.LookAt += translationX;
                    break;
                case ViewTypes.Y:
                    var translationY = new Vector3()
                    {
                        X = position.X - Camera.Position.X,
                        Y = 0.0f,
                        Z = position.Z - Camera.Position.Z
                    };

                    Camera.Position += translationY;
                    Camera.LookAt += translationY;
                    break;
                case ViewTypes.Z:
                    var translationZ = new Vector3()
                    {
                        X = position.X - Camera.Position.X,
                        Y = position.Y - Camera.Position.Y,
                        Z = 0.0f
                    };

                    Camera.Position += translationZ;
                    Camera.LookAt += translationZ;
                    break;
            }
        }

        public bool Zoom(int wheelDelta)
        {
            if (Camera is OrthographicCamera camera)
            {
                var width = camera.Width - wheelDelta * 0.01f;

                if (width > 0.0f)
                {
                    camera.Width = width;
                    return true;
                }
            }

            return false;
        }

        public void Load()
        {
            switch (ViewType)
            {
                case ViewTypes.Perspective:
                    Camera = new PerspectiveCamera("", 0.1f, 1000.0f, UnitConversions.ToRadians(45.0f));
                    Camera.DetachFromEntity();
                    Camera.Position = new Vector3(0.0f, -10.0f, 10.0f);
                    Camera.Up = Vector3.UnitZ;
                    Camera.LookAt = Camera.Position + Vector3.UnitY;
                    _yaw = MathExtensions.HALF_PI;
                    _pitch = 0.0f;
                    break;
                case ViewTypes.X:
                    Camera = new OrthographicCamera("", -1000.0f, 1000.0f, 20.0f)
                    {
                        Position = Vector3.UnitX * -100.0f//new Vector3(_map.Boundaries.Min.X - 10.0f, 0.0f, 0.0f),
                    };
                    Camera.Up = Vector3.UnitZ;
                    Camera.LookAt = Camera.Position + Vector3.UnitX;
                    _gridRenderer.RotateGrid(0.0f, MathExtensions.HALF_PI, 0.0f);
                    _yaw = 0.0f;
                    _pitch = 0.0f;
                    break;
                case ViewTypes.Y:
                    Camera = new OrthographicCamera("", -1000.0f, 1000.0f, 20.0f)
                    {
                        Position = Vector3.UnitY * -100.0f,//new Vector3(0.0f, _map.Boundaries.Min.Y - 10.0f, 0.0f),
                    };
                    Camera.Up = Vector3.UnitZ;
                    Camera.LookAt = Camera.Position + Vector3.UnitY;
                    _gridRenderer.RotateGrid(MathExtensions.HALF_PI, 0.0f, 0.0f);
                    _yaw = MathExtensions.HALF_PI;
                    _pitch = 0.0f;
                    break;
                case ViewTypes.Z:
                    Camera = new OrthographicCamera("", -1000.0f, 1000.0f, 20.0f)
                    {
                        Position = Vector3.UnitZ * 100.0f,//new Vector3(0.0f, 0.0f, _map.Boundaries.Max.Z + 10.0f),
                    };
                    Camera.Up = Vector3.UnitY;
                    Camera.LookAt = Camera.Position - Vector3.UnitZ;
                    _yaw = 0.0f;
                    _pitch = -MathExtensions.HALF_PI;
                    break;
            }

            _entityProvider.AddEntity(Camera);
            Camera.IsActive = true;
        }

        public void Travel(Vector2 mouseDelta)
        {
            // Left mouse button allows "moving"
            if (mouseDelta != Vector2.Zero)
            {
                var translation = (Camera.LookAt - Camera.Position) * mouseDelta.Y * 0.02f;
                Camera.Position -= translation;
                
                _yaw = (_yaw - mouseDelta.X * 0.001f) % MAX_YAW;
                CalculateLookAt();
                CalculateUp();
            }
        }

        public void Turn(Vector2 mouseDelta)
        {
            // Right mouse button allows "turning"
            if (mouseDelta != Vector2.Zero)
            {
                _yaw = (_yaw - mouseDelta.X * 0.001f) % MAX_YAW;
                _pitch -= mouseDelta.Y * 0.001f;
                _pitch = _pitch.Clamp(MIN_PITCH, MAX_PITCH);

                CalculateLookAt();
                CalculateUp();
            }
        }

        public void Strafe(Vector2 mouseDelta)
        {
            // Both mouse buttons allow "strafing"
            if (mouseDelta != Vector2.Zero)
            {
                var upDirection = Camera.Up;
                var lookDirection = Camera.LookAt - Camera.Position;

                var rightDirection = Vector3.Cross(upDirection, lookDirection).Normalized();

                var verticalTranslation = upDirection * mouseDelta.Y * 0.02f;
                var horizontalTranslation = rightDirection * mouseDelta.X * 0.02f;

                Camera.Position -= verticalTranslation + horizontalTranslation;
                Camera.LookAt -= verticalTranslation + horizontalTranslation;
            }
        }

        public void Pivot(Vector2 mouseDelta, int mouseWheelDelta, Vector3 position)
        {
            if (ViewType == ViewTypes.Perspective && (mouseDelta != Vector2.Zero || mouseWheelDelta != 0))
            {
                // Determine new yaw and pitch
                var lookDirection = (position - Camera.Position).Normalized();
                _pitch = (float)Math.Asin(lookDirection.Z);
                _yaw = ((float)Math.Atan2(lookDirection.Y, lookDirection.X) + MAX_YAW) % MAX_YAW;


                if (mouseWheelDelta != 0.0f)
                {
                    var translation = lookDirection * mouseWheelDelta * 1.0f;
                    Camera.Position -= translation;
                }

                if (mouseDelta != Vector2.Zero)
                {
                    // Now, we can adjust our position accordingly
                    _yaw = (_yaw + mouseDelta.X * 0.001f) % MAX_YAW;//_yaw += mouseDelta.X * 0.001f;
                    _pitch += mouseDelta.Y * 0.001f;
                    _pitch = _pitch.Clamp(MIN_PITCH, MAX_PITCH);
                }

                CalculateTranslation(position);
                CalculateUp();
            }
        }

        private void CalculateTranslation(Vector3 position)
        {
            var distance = (Camera.Position - position).Length;

            var translation = new Vector3()
            {
                X = distance * (float)(Math.Cos(_pitch) * Math.Cos(_yaw)),
                Y = distance * (float)(Math.Cos(_pitch) * Math.Sin(_yaw)),
                Z = distance * (float)Math.Sin(_pitch)
            };

            Camera.Position = position - translation;
            Camera.LookAt = Camera.Position + translation / distance;
        }

        private void CalculateLookAt()
        {
            var lookDirection = new Vector3()
            {
                X = (float)(Math.Cos(_yaw) * Math.Cos(_pitch)),
                Y = (float)(Math.Sin(_yaw) * Math.Cos(_pitch)),
                Z = (float)Math.Sin(_pitch)
            };


            Camera.LookAt = Camera.Position + lookDirection.Normalized();
        }

        private void CalculateUp()
        {
            var yAngle = _pitch + MathExtensions.HALF_PI;
            //var yAngle = ((_pitch - (float)Math.PI / 2.0f) + 2.0f * (float)Math.PI) % (2.0f * (float)Math.PI);

            var upDirection = new Vector3()
            {
                X = (float)(Math.Cos(_yaw) * Math.Cos(yAngle)),
                Y = (float)(Math.Sin(_yaw) * Math.Cos(yAngle)),
                Z = (float)Math.Sin(yAngle)
            };

            Camera.Up = upDirection.Normalized();
        }
    }
}
