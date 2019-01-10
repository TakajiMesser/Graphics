using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Inputs;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using Brush = SpiceEngine.Entities.Brushes.Brush;
using Timer = System.Timers.Timer;
using SpiceEngine.Rendering;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Physics;
using SpiceEngine.Scripting;
using SpiceEngine.Sounds;
using SpiceEngine.Rendering.Textures;
using System.IO;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Vertices;

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
        public const float MAX_ANGLE_Y = (float)Math.PI / 2.0f + 0.1f;
        public const float MIN_ANGLE_Y = -(float)Math.PI / 2.0f + 0.1f;

        private Resolution _resolution;
        private RenderManager _renderManager;

        private Vector3 _currentAngles = new Vector3();

        public Camera Camera { get; private set; }
        public ViewTypes ViewType { get; set; }

        public PanelCamera(Resolution resolution, RenderManager renderManager)
        {
            _resolution = resolution;
            _renderManager = renderManager;
        }

        public void CenterView(IEnumerable<Vector3> positions)
        {
            switch (ViewType)
            {
                case ViewTypes.Perspective:
                    break;
                case ViewTypes.X:
                    var translationX = new Vector3()
                    {
                        X = 0.0f,
                        Y = positions.Average(p => p.Y) - Camera.Position.Y,
                        Z = positions.Average(p => p.Z) - Camera.Position.Z
                    };

                    Camera.Position += translationX;
                    Camera._viewMatrix.LookAt += translationX;
                    break;
                case ViewTypes.Y:
                    var translationY = new Vector3()
                    {
                        X = positions.Average(p => p.X) - Camera.Position.X,
                        Y = 0.0f,
                        Z = positions.Average(p => p.Z) - Camera.Position.Z
                    };

                    Camera.Position += translationY;
                    Camera._viewMatrix.LookAt += translationY;
                    break;
                case ViewTypes.Z:
                    var translationZ = new Vector3()
                    {
                        X = positions.Average(p => p.X) - Camera.Position.X,
                        Y = positions.Average(p => p.Y) - Camera.Position.Y,
                        Z = 0.0f
                    };

                    Camera.Position += translationZ;
                    Camera._viewMatrix.LookAt += translationZ;
                    break;
            }
        }

        public boolean Zoom(int wheelDelta)
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

        private void Load()
        {
            switch (ViewType)
            {
                case ViewTypes.Perspective:
                    Camera = new PerspectiveCamera("", Resolution, 0.1f, 1000.0f, UnitConversions.ToRadians(45.0f));
                    Camera.DetachFromEntity();
                    _currentAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    Camera._viewMatrix.Up = Vector3.UnitZ;
                    Camera._viewMatrix.LookAt = Camera.Position + Vector3.UnitY;
                    break;
                case ViewTypes.X:
                    Camera = new OrthographicCamera("", Resolution, -1000.0f, 1000.0f, 20.0f)
                    {
                        Position = Vector3.UnitX * -100.0f//new Vector3(_map.Boundaries.Min.X - 10.0f, 0.0f, 0.0f),
                    };
                    _currentAngles = new Vector3(90.0f, 0.0f, 0.0f);
                    Camera._viewMatrix.Up = Vector3.UnitZ;
                    Camera._viewMatrix.LookAt = Camera.Position + Vector3.UnitX;
                    _renderManager.RotateGrid(0.0f, (float)Math.PI / 2.0f, 0.0f);
                    break;
                case ViewTypes.Y:
                    Camera = new OrthographicCamera("", Resolution, -1000.0f, 1000.0f, 20.0f)
                    {
                        Position = Vector3.UnitY * -100.0f,//new Vector3(0.0f, _map.Boundaries.Min.Y - 10.0f, 0.0f),
                    };
                    _currentAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    Camera._viewMatrix.Up = Vector3.UnitZ;
                    Camera._viewMatrix.LookAt = Camera.Position + Vector3.UnitY;
                    _renderManager.RotateGrid(0.0f, 0.0f, (float)Math.PI / 2.0f);
                    break;
                case ViewTypes.Z:
                    Camera = new OrthographicCamera("", Resolution, -1000.0f, 1000.0f, 20.0f)
                    {
                        Position = Vector3.UnitZ * 100.0f,//new Vector3(0.0f, 0.0f, _map.Boundaries.Max.Z + 10.0f),
                    };
                    _currentAngles = new Vector3(0.0f, 90.0f, 0.0f);
                    Camera._viewMatrix.Up = Vector3.UnitY;
                    Camera._viewMatrix.LookAt = Camera.Position - Vector3.UnitZ;
                    break;
            }
        }

        public void Strafe(Vector2 mouseDelta)
        {
            // Both mouse buttons allow "strafing"
            var right = Vector3.Cross(Camera._viewMatrix.Up, Camera._viewMatrix.LookAt - Camera.Position).Normalized();

            var verticalTranslation = Camera._viewMatrix.Up * _inputManager.MouseDelta.Y * 0.02f;
            var horizontalTranslation = right * _inputManager.MouseDelta.X * 0.02f;

            Camera.Position -= verticalTranslation + horizontalTranslation;
            Camera._viewMatrix.LookAt -= verticalTranslation + horizontalTranslation;
        }

        public void Travel(Vector2 mouseDelta)
        {
            // Left mouse button allows "moving"
            var translation = (Camera._viewMatrix.LookAt - Camera.Position) * mouseDelta.Y * 0.02f;
            //var translation = new Vector3(_gameState._camera._viewMatrix.LookAt.X - _gameState._camera.Position.X, _gameState._camera._viewMatrix.LookAt.Y - _gameState._camera.Position.Y, 0.0f) * _inputManager.MouseDelta.Y * 0.02f;
            Camera.Position -= translation;

            var currentAngles = _currentAngles;
            mouseDelta *= 0.001f;

            if (mouseDelta != Vector2.Zero)
            {
                currentAngles.X += mouseDelta.X;
                _currentAngles = currentAngles;

                CalculateDirection();
            }

            CalculateUp();
        }

        public void Turn(Vector2 mouseDelta)
        {
            // Right mouse button allows "turning"
            var currentAngles = _currentAngles;
            mouseDelta *= 0.001f;

            if (mouseDelta != Vector2.Zero)
            {
                currentAngles.X += mouseDelta.X;
                currentAngles.Y += mouseDelta.Y;
                currentAngles.Y = currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);
                _currentAngles = currentAngles;

                CalculateDirection();
                CalculateUp();
            }
        }

        public void Pivot(Vector2 mouseDelta, IEnumerable<Vector3> positions)
        {
            if (ViewType == ViewTypes.Perspective)
            {
                mouseDelta *= 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    currentAngles.X += mouseDelta.X;
                    currentAngles.Y += mouseDelta.Y;
                    _currentAngles = currentAngles;
                    //_currentAngles.Y = _currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);

                    var position = new Vector3()
                    {
                        X = positions.Average(p => p.X),
                        Y = positions.Average(p => p.Y),
                        Z = positions.Average(p => p.Z) 
                    };

                    CalculateTranslation(position);
                    CalculateUp();
                }
            }
        }

        private void CalculateTranslation(Vector3 position)
        {
            var horizontal = _distance * Math.Cos(_currentAngles.Y);
            var vertical = _distance * Math.Sin(_currentAngles.Y);

            var translation = new Vector3()
            {
                X = -(float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = -(float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = (float)vertical
            };

            Camera.Position = position - translation;
            Camera._viewMatrix.LookAt = position;
        }

        private void CalculateDirection()
        {
            var horizontal = Math.Cos(_currentAngles.Y);
            var vertical = Math.Sin(_currentAngles.Y);

            Camera._viewMatrix.LookAt = Camera.Position + new Vector3()
            {
                X = (float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = (float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = -(float)vertical
            };
        }

        private void CalculateUp()
        {
            var yAngle = _currentAngles.Y - (float)Math.PI / 2.0f;

            var horizontal = Math.Cos(yAngle);
            var vertical = Math.Sin(yAngle);

            Camera._viewMatrix.Up = new Vector3()
            {
                X = (float)(horizontal * Math.Sin(_currentAngles.X)),
                Y = (float)(horizontal * Math.Cos(_currentAngles.X)),
                Z = -(float)vertical
            };
        }
    }
}
