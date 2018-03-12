using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using TakoEngine;
using TakoEngine.Meshes;
using TakoEngine.GameObjects;
using System.Runtime.InteropServices;
using OpenTK.Input;
using TakoEngine.Physics.Collision;
using TakoEngine.Helpers;
using TakoEngine.Maps;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Rendering.PostProcessing;
using TakoEngine.Outputs;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Forms;
using TakoEngine.Inputs;
using TakoEngine.Rendering.Processing;
using TakoEngine.Rendering.Textures;
using TakoEngine.Lighting;
using TakoEngine.Utilities;

namespace TakoEngine.GameObjects
{
    public class GamePanel : OpenTK.GLControl
    {
        public const float MAX_ANGLE_Y = (float)Math.PI / 2.0f + 0.1f;
        public const float MIN_ANGLE_Y = -(float)Math.PI / 2.0f + 0.1f;

        public RenderModes RenderMode { get; set; }
        public double Frequency { get; private set; }
        public Resolution Resolution { get; private set; }
        public EventHandler Resized;
        public bool IsMouseInPanel { get; private set; }
        public bool IsCameraMoving { get; private set; }

        private string _mapPath;
        private GameState _gameState;

        private Vector3 CurrentAngles { get; set; } = new Vector3();

        internal InputState _inputState = new InputState();

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private System.Timers.Timer _fpsTimer = new System.Timers.Timer(1000);
        private List<double> _frequencies = new List<double>();

        private object _cursorLock = new object();
        private bool _isCursorVisible = true;
        public bool IsCursorVisible
        {
            get
            {
                lock (_cursorLock)
                {
                    return _isCursorVisible;
                }
            }
            set
            {
                lock (_cursorLock)
                {
                    if (value != _isCursorVisible)
                    {
                        if (value)
                        {
                            Cursor.Show();
                        }
                        else
                        {
                            Cursor.Hide();
                            //Cursor.Position = new Point(0, 0);
                        }

                        _isCursorVisible = value;
                    }
                }
            }
        }

        public GamePanel(string mapPath) : base(GraphicsMode.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Resolution = new Resolution(Width, Height);

            _mapPath = mapPath;
            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));

            _fpsTimer.Elapsed += (s, e) =>
            {
                if (_frequencies.Count > 0)
                {
                    _gameState?.SetFrequency(_frequencies.Average());
                    _frequencies.Clear();
                }
            };
            _fpsTimer.Start();

            _timer.Interval = 16.67;
            _timer.Elapsed += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnUpdateFrame();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            MakeCurrent();
            OnRenderFrame();
        }

        protected override void OnResize(EventArgs e)
        {
            if (Resolution != null)
            {
                Resolution.Width = Width;
                Resolution.Height = Height;
                _gameState?.Resize();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(0, 0);

            var loadedMap = Map.Load(_mapPath);

            _gameState = new GameState(loadedMap, Resolution);
            _gameState.Initialize();
            _gameState._camera.DetachFromGameObject();

            // Set camera to default position when _horizontalAngle = 0 and _verticalAngle = 0
            _gameState._camera._viewMatrix.Up = Vector3.UnitZ;
            _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position + Vector3.UnitY;

            _timer.Start();
        }

        protected void OnUpdateFrame()
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            HandleInput();
            //_gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
            PollForInput();
        }

        protected void OnRenderFrame()
        {
            _frequencies.Add(Frequency);

            switch (RenderMode)
            {
                case RenderModes.Wireframe:
                    _gameState.RenderWireframe();
                    break;
                case RenderModes.Diffuse:
                    _gameState.RenderDiffuseFrame();
                    break;
                case RenderModes.Lit:
                    _gameState.RenderLitFrame();
                    break;
                case RenderModes.Full:
                    _gameState.RenderFullFrame();
                    break;
            }

            GL.UseProgram(0);
            SwapBuffers();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            IsMouseInPanel = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            IsMouseInPanel = false;
            base.OnMouseLeave(e);
        }

        private void CalculateDirection()
        {
            var horizontal = Math.Cos(CurrentAngles.Y);
            var vertical = Math.Sin(CurrentAngles.Y);

            _gameState._camera._viewMatrix.LookAt = _gameState._camera.Position + new Vector3()
            {
                X = (float)(horizontal * Math.Sin(CurrentAngles.X)),
                Y = (float)(horizontal * Math.Cos(CurrentAngles.X)),
                Z = -(float)vertical
            };
        }

        private void CalculateUp()
        {
            var yAngle = CurrentAngles.Y - (float)Math.PI / 2.0f;

            var horizontal = Math.Cos(yAngle);
            var vertical = Math.Sin(yAngle);

            _gameState._camera._viewMatrix.Up = new Vector3()
            {
                X = (float)(horizontal * Math.Sin(CurrentAngles.X)),
                Y = (float)(horizontal * Math.Cos(CurrentAngles.X)),
                Z = -(float)vertical
            };
        }

        private void HandleInput()
        {
            if ((_gameState._inputState.IsPressed(new Input(MouseButton.Left)) && _gameState._inputState.IsPressed(new Input(MouseButton.Right)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Left)) && _gameState._inputState.IsHeld(new Input(MouseButton.Right)) && IsCameraMoving))
            {
                // Both mouse buttons allow "strafing"
                var right = Vector3.Cross(_gameState._camera._viewMatrix.Up, _gameState._camera._viewMatrix.LookAt - _gameState._camera.Position).Normalized();

                var verticalTranslation = _gameState._camera._viewMatrix.Up * _gameState._inputState.MouseDelta.Y * 0.02f;
                var horizontalTranslation = right * _gameState._inputState.MouseDelta.X * 0.02f;

                _gameState._camera.Position -= verticalTranslation + horizontalTranslation;
                _gameState._camera._viewMatrix.LookAt -= verticalTranslation + horizontalTranslation;
                IsCameraMoving = true;
                IsCursorVisible = false;
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Left)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Left)) && IsCameraMoving))
            {
                // Left mouse button allows "moving"
                var translation = (_gameState._camera._viewMatrix.LookAt - _gameState._camera.Position) * _gameState._inputState.MouseDelta.Y * 0.02f;
                //var translation = new Vector3(_gameState._camera._viewMatrix.LookAt.X - _gameState._camera.Position.X, _gameState._camera._viewMatrix.LookAt.Y - _gameState._camera.Position.Y, 0.0f) * _gameState._inputState.MouseDelta.Y * 0.02f;
                _gameState._camera.Position -= translation;

                var currentAngles = CurrentAngles;
                var mouseDelta = _gameState._inputState.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    currentAngles.X += mouseDelta.X;
                    CurrentAngles = currentAngles;

                    CalculateDirection();
                }

                CalculateUp();

                IsCameraMoving = true;
                IsCursorVisible = false;
            }
            else if ((_gameState._inputState.IsPressed(new Input(MouseButton.Right)) && IsMouseInPanel)
                    || (_gameState._inputState.IsHeld(new Input(MouseButton.Right)) && IsCameraMoving))
            {
                // Right mouse button allows "turning"
                var currentAngles = CurrentAngles;
                var mouseDelta = _gameState._inputState.MouseDelta * 0.001f;

                if (mouseDelta != Vector2.Zero)
                {
                    currentAngles.X += mouseDelta.X;
                    currentAngles.Y += mouseDelta.Y;
                    currentAngles.Y = currentAngles.Y.Clamp(MIN_ANGLE_Y, MAX_ANGLE_Y);
                    CurrentAngles = currentAngles;

                    CalculateDirection();
                    CalculateUp();
                }
                
                IsCameraMoving = true;
                IsCursorVisible = false;
            }
            else
            {
                IsCameraMoving = false;
                IsCursorVisible = true;
            }
        }

        private Point _mouseLocation;

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            _mouseLocation = e.Location;
        }

        private void PollForInput()
        {
            _gameState._inputState.UpdateState(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
